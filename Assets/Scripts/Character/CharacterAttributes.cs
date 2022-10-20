using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterAttributeItems
{
	// Basics
	public const float MAX_HEALTH = 100f;
	public float Health = MAX_HEALTH;
	
	// Movement
	public float MovementSpeedMultiplier = 1.0f;
	
	// Weapons
	public List<GameObject> equippedWeapons = new List<GameObject>();
	public int activeWeaponIndex = 0;
	
	// Camera effects
	public bool ExplosionShakeEnabled = false;
	public bool TimeDilationEnabled = false;
	
	// Skills
	public bool SkillHasGrenade = false;

	// UnderAttack
	public bool IsUnderAttack = false;
}

// This class implements the character attributes & modifier system
public class CharacterAttributes : MonoBehaviour
{

	public CharacterAttributeItems characterAttributes = new CharacterAttributeItems();

	// Character modifier list
	public List<CharacterModifier> Modifiers = new List<CharacterModifier>();
	
	// Update is called once per frame, updates modifiers
    void Update()
	{
		foreach (CharacterModifier modifier in Modifiers) // Iterate over attributes
		{
			if (modifier.DurationRemaining <= 0) { // Attribute duration finished, remove attribute
				RemoveModifier(modifier);
			} else if (modifier.State != ModifierState.REMOVED) {
				modifier.DurationRemaining -= Time.deltaTime; // Decrement duration
				modifier.OnUpdateModifier(this.characterAttributes);
			}
		}
        
	}

	public void Attacked(CharacterModifier modifier)
    {
		this.characterAttributes.IsUnderAttack = true;

	}
	
	// Adds a modifier to the character
	public void AddModifier(CharacterModifier modifier) {
		modifier.DurationRemaining = modifier.Duration; // Initialize remaining duration
		modifier.OnAddModifier(this.characterAttributes); // Trigger modifier on add function
		Modifiers.Add(modifier); // Add modifier to list of modifiers
	}
	
	// Removes a modifier from the character by modifier
	public void RemoveModifier(CharacterModifier modifier) {
		Modifiers.Remove(modifier);
		modifier.OnRemoveModifier(this.characterAttributes);
		
	}
	
	// Removes a modifier from the character by modifier guid
	public void RemoveModifier(Guid modifierId) {
		CharacterModifier modifierToRemove = Modifiers.Find(modifier => modifier.Id == modifierId);
		
		if (modifierToRemove !=  null)
		{
			Modifiers.Remove(modifierToRemove);
			modifierToRemove.OnRemoveModifier(this.characterAttributes);
		}
		
	}
	
	// Removes all modifiers from a character
	public void RemoveAllModifiers(string effectedAttribute = null) {
		foreach (CharacterModifier modifier in Modifiers)
		{
			if (effectedAttribute == null || modifier.EffectedAttributes.Contains(effectedAttribute)) // If property filter not set or equals the property
			{
				RemoveModifier(modifier);
			}
		}
		
	}
}
