﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
	bool isTriggered = false;
	
	public int amount = 50;
	
	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.GetComponent<CharacterAttributes>() != null && !isTriggered)
		{
			isTriggered = true;
			Destroy(this.gameObject); // Remove health pack
			c.gameObject.GetComponent<CharacterAttributes>().AddModifier(new HealthModifier(
				healthAmount: amount,
				trigger: ModifierTrigger.ON_ADD
			)); // Add a +50 health modifier to the player
		}
	}
}
