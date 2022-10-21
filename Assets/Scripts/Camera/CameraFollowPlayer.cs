using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
	public Transform player;
	public float smoothSpeed = 0.1f;
	private Vector3 velocity = Vector3.zero;

	void LateUpdate() 
	{
		// Handle rotating camera around player
		transform.position = Vector3.SmoothDamp(transform.position,
			player.position,
			ref velocity,
			smoothSpeed * Time.deltaTime);
		Debug.Log("Player roation: " + player.rotation);
		transform.rotation = Quaternion.Slerp(transform.rotation,
			player.rotation, //Mathf.Deg2Rad30),
			smoothSpeed * 29 * Time.deltaTime) ;
	}
}
