using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	private GameObject player;
	public bool isDoorOpen;
	private Animator _animator;
	
    // Start is called before the first frame update
    void Start()
    {
	    player = GameObject.Find("Player");
	    _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
	void LateUpdate()
	{
		// the player is within a radius of 3 units to this game object
		if ((player.transform.position-this.transform.position).sqrMagnitude < 5*5 && !isDoorOpen) {
			Debug.Log("Door Opening");
			_animator.SetTrigger("OnDoorOpen");
			isDoorOpen = true;
		} else if ((player.transform.position-this.transform.position).sqrMagnitude >= 5*5 && isDoorOpen) { // No player nearby
			Debug.Log("Door Closing");
	    	_animator.SetTrigger("OnDoorClose");
	    	isDoorOpen = false;
	    }
    }
}