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
			this.player.GetComponent<CharacterAttributes>().AddModifier(new HealthModifier(healthAmount: 50, trigger: ModifierTrigger.ON_ADD)); // Add a +50 health modifier to the player
		}
	}
}
