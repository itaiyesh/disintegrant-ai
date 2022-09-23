using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public Transform cameraParent;
	public float rotationSpeed;

	void LateUpdate()
	{
		// Handle rotating camera around player
		if (Input.GetKey(KeyCode.E))
		{
			transform.RotateAround(cameraParent.position, Vector3.up, rotationSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.Q))
		{
			transform.RotateAround(cameraParent.position, -Vector3.up, rotationSpeed * Time.deltaTime);
		}
	}
}
