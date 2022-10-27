using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLeechCollectable : MonoBehaviour
{
	bool isTriggered = false;
	
	public int amount = -100;
	public float duration = 5.0f;
	
	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.GetComponent<CharacterAttributes>() != null && !isTriggered)
		{
			isTriggered = true;
			Destroy(this.gameObject); // Remove health pack
			HealthModifier pickup = new HealthModifier(
				healthAmount: amount,
				duration: duration,
				trigger: ModifierTrigger.ON_UPDATE); // Create health leech that reduces 100 health over 5 seconds
			c.gameObject.GetComponent<CharacterAttributes>().AddModifier(pickup); // Add the modifier to the player
		}
	}
}


