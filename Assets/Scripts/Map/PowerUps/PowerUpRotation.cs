using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpRotation : MonoBehaviour
{

	#region Settings
	public float rotationSpeed = 99.0f;
	public bool reverse = false;

    public bool x = false;
    public bool y = false;
    public bool z = true;

    #endregion

    void Update()
    {
        if (x == true)
        {
            if (this.reverse)
                transform.Rotate(new Vector3(1f, 0f, 0f) * Time.deltaTime * this.rotationSpeed);
            else
                transform.Rotate(new Vector3(1f, 0f, 0f) * Time.deltaTime * this.rotationSpeed);
        }
        if (y == true)
        {
            if (this.reverse)
                transform.Rotate(new Vector3(0f, 1f, 0f) * Time.deltaTime * this.rotationSpeed);
            else
                transform.Rotate(new Vector3(0f, 1f, 0f) * Time.deltaTime * this.rotationSpeed);
        }
        if (z == true)
        {
            if (this.reverse)
                transform.Rotate(new Vector3(0f, 0f, 1f) * Time.deltaTime * this.rotationSpeed);
            else
                transform.Rotate(new Vector3(0f, 0f, 1f) * Time.deltaTime * this.rotationSpeed);
        }

	}

	public void SetRotationSpeed(float speed)
	{
		this.rotationSpeed = speed;
	}

	public void SetReverse(bool reverse)
	{
		this.reverse = reverse;
	}
}
