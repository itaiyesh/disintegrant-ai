using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public bool isDoorOpen = false;
	private Animator _animator;
	
	void Awake()
	{
		_animator = GetComponent<Animator>();
	}
	
	void OnTriggerEnter(Collider c)
	{
		if (c.attachedRigidbody != null && !isDoorOpen)
		{
			_animator.SetTrigger("OnDoorOpen");
			isDoorOpen = true;
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if (c.attachedRigidbody != null && isDoorOpen)
		{
			_animator.SetTrigger("OnDoorClose");
			isDoorOpen = false;
		}
	}
}