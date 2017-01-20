using UnityEngine;
using System.Collections;

public class HeroCameraController : MonoBehaviour {


	public Transform target;
	public float correctUp = 1;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float distanceMin = .5f;
	public float distanceMax = 15f;
	float newDistance = 0;

	private new Rigidbody rigidbody;

	float x = 0.0f;
	public bool handle;
	float y = 0.0f;

	// Use this for initialization
	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		newDistance = distanceMax;
		rigidbody = GetComponent<Rigidbody>();

		if (rigidbody != null)
		{
			rigidbody.freezeRotation = true;
		}
	}

	void LateUpdate()
	{
		if (target && handle)
		{
			x += Input.GetAxis("Horizontal") * xSpeed * Time.deltaTime;
			y -= Input.GetAxis("Vertical") * ySpeed * Time.deltaTime;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y, x, 0);


			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceMax);
			Vector3 position = rotation * negDistance + target.position + Vector3.up * correctUp;
			RaycastHit hit;
			if (Physics.Linecast(target.position, position, out hit, LayerMask.GetMask("Map")))
			{
				if (hit.collider.transform != transform && hit.collider.transform != transform.parent)
				{
					//Debug.Log(hit.collider.name + " : " + hit.distance);
					newDistance = hit.distance - 1;
                    if (newDistance < distanceMin)
						newDistance = distanceMin;
				}
				else
					newDistance = distanceMax;
			}
			else
				newDistance = distanceMax;

			distance = Mathf.Lerp(distance, newDistance, Time.deltaTime * 20);
			negDistance = new Vector3(0.0f, 0.0f, -distance);
			position = rotation * negDistance + target.position + Vector3.up * correctUp;

			transform.rotation = rotation;
			transform.position = position;
			//transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
			handle = false;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
