using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectable : MonoBehaviour
{
	bool isTriggered = false;
	
	Weapon weapon;
	
	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.GetComponent<CharacterAttributes>() != null && !isTriggered)
		{
			isTriggered = true;
			
			GameObject weapon = transform.GetChild(0).gameObject;
			
			// Check if the player already has a weapon of the same type
			if (c.gameObject.GetComponent<CharacterAttributes>().characterAttributes.equippedWeapons.Exists(wep => wep.GetComponent<Weapon>().Name == weapon.GetComponent<Weapon>().Name)) 
			{
				isTriggered = false;
				return;
			}
			
			// Add weapon to player if it doesn't already exist
			c.gameObject.GetComponent<WeaponController>().AddWeapon(weapon); // Add the modifier to the player
			Destroy(this.gameObject);
		}
	}
}


