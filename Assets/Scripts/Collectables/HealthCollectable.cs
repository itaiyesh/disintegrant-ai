using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
	GameObject player;
	bool isTriggered = false;
	
	void Awake()
	{
		this.player = GameObject.FindWithTag("Player");
	}
	
	void OnTriggerEnter(Collider c)
	{
		if (c.attachedRigidbody != null && !isTriggered)
		{
			isTriggered = true;
			Destroy(this.gameObject); // Remove health pack
			HealthModifier pickup = new HealthModifier(50); // Create health pickup that adds 30 health to player
			this.player.GetComponent<CharacterAttributes>().AddModifier(pickup); // Add the modifier to the player
		}
	}
}
