using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLeechCollectable : MonoBehaviour
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
			HealthLeech pickup = new HealthLeech(100, 5); // Create health leech that reduces 100 health over n milliseconds
			this.player.GetComponent<CharacterAttributes>().AddModifier(pickup); // Add the modifier to the player
		}
	}
}
