﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectable : MonoBehaviour
{
	bool isTriggered = false;
	
	Weapon weapon;
	
	public void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.GetComponent<CharacterAttributes>() != null && !isTriggered)
		{
			isTriggered = true;
			
			GameObject weapon = transform.GetChild(0).gameObject;
			
			GameObject playerWeapon = c.gameObject.GetComponent<CharacterAttributes>().characterAttributes.equippedWeapons.Find(wep => wep.GetComponent<Weapon>().Name == weapon.GetComponent<Weapon>().Name);
			
			// Check if the player already has a weapon of the same type
			if (playerWeapon != null) 
			{
				// Add collectables ammo to the player's weapon
				playerWeapon.GetComponent<Weapon>().Ammo += weapon.GetComponent<Weapon>().Ammo;
				// Trigger add event so ammo is updated on hud
				EventManager.TriggerEvent<WeaponAddEvent, GameObject, GameObject>(c.gameObject, playerWeapon);
				Destroy(this.gameObject);
				return;
			}
			
			// Add weapon to player if it doesn't already exist
			c.gameObject.GetComponent<WeaponController>().AddWeapon(weapon); // Add the modifier to the player
			Destroy(this.gameObject);
		}
	}
}


