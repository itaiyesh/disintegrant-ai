using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class HealthCollectable : MonoBehaviour
{
	bool isTriggered = false;
	
	public int amount = 50;
	
	void OnTriggerEnter(Collider c)
	{
		var characterAttribute = c.gameObject.GetComponent<CharacterAttributes>();
		if (characterAttribute != null && !isTriggered)
		{
			isTriggered = true;
			Destroy(gameObject); // Remove health pack

			// Don't modify health if value is already at max.
			if (characterAttribute.characterAttributes.Health >= CharacterAttributeItems.MAX_HEALTH) return;

			var healthAmountToAdd = System.Math.Min(
				CharacterAttributeItems.MAX_HEALTH - characterAttribute.characterAttributes.Health,
				CharacterAttributeItems.MAX_HEALTH
			);
			
			characterAttribute.AddModifier(new HealthModifier(
				healthAmount: healthAmountToAdd,
				trigger: ModifierTrigger.ON_ADD
			)); // Add a +50 health modifier to the player
		}
	}
}
