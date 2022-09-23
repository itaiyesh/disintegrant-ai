using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public GameObject player;
	private Vector3 offset;

	// Start is called before the first frame update
	void Start()
	{
		offset = transform.position - player.transform.position;
	}
	
	void Update()
	{
		if(Input.GetKey (KeyCode.E))
		{
			transform.RotateAround(transform.position, Vector3.up, 30 * Time.deltaTime);
		}

		if(Input.GetKey (KeyCode.Q))
		{
			transform.RotateAround(transform.position, -Vector3.up, 30 * Time.deltaTime);
		}
	}

	// Update is called once per frame
	//void LateUpdate()
	//{
	//	transform.position = player.transform.position + offset;
	//}
}
