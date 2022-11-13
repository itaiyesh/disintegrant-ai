using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorScript : MonoBehaviour
{
	public bool isDoorOpen = false;
	private Animator anim;
	
	void Awake()
	{
		anim = GetComponent<Animator>();
		
		if (anim == null)
			Debug.Log("Animator could not be found");
	}
	
	void OnTriggerEnter(Collider c)
	{
		if (c.attachedRigidbody != null && !isDoorOpen)
		{
			anim.SetTrigger("OnDoorOpen");
			isDoorOpen = true;
			GetComponent<MeshCollider>().enabled = false;
			GetComponent<BoxCollider>().enabled = false;
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if (c.attachedRigidbody != null && isDoorOpen)
		{
			anim.SetTrigger("OnDoorClose");
			isDoorOpen = false;
			GetComponent<MeshCollider>().enabled = true;
			GetComponent<BoxCollider>().enabled = true;
		}
	}
}