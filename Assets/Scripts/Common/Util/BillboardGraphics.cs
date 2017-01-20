using UnityEngine;
using System.Collections;

public class BillboardGraphics : MonoBehaviour {

	private Camera cam;
	// Use this for initialization
	void Start()
	{
		cam = Camera.main;
		if (cam != null)
			transform.forward = cam.transform.forward ;

	}

	// Update is called once per frame
	void Update()
	{
		if (cam != null)
			transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
		else if (Camera.allCamerasCount > 0)
			cam = Camera.allCameras[0];
		/*if (cam != null)
		transform.forward = cam.transform.forward;
		else
		cam = Camera.main;*/
	}
}
