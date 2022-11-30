using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterAttributeItems : ICloneable
{
	// Basics
	public bool IsAlive { get; set; } = true;
	public const float MAX_HEALTH = 100f;

	// Backing field for health
	private float _health = MAX_HEALTH;
	public float Health
	{
		get => Mathf.Clamp(_health, 0, MAX_HEALTH);
		set => _health = value;
	}
	
	// Movement
	public float MovementSpeedMultiplier { get; set; } = 1.0f;
	
	// Weapons
	public List<GameObject> equippedWeapons { get; set; } = new List<GameObject>();
	public int activeWeaponIndex { get; set; } = 0;
	
	// Camera effects
	public bool ExplosionShakeEnabled { get; set; } = false;
	public bool TimeDilationEnabled { get; set; } = false;
	
	// Skills
	public bool SkillHasGrenade { get; set; } = false;

	// UnderAttack
	public bool IsUnderAttack { get; set; } = false;
	
	public object Clone()
	{
		return this.MemberwiseClone();
	}
}

// This class implements the character attributes & modifier system
public class CharacterAttributes : MonoBehaviour
{

	public CharacterAttributeItems characterAttributes = new CharacterAttributeItems();

	// Character modifier list
	public List<CharacterModifier> Modifiers = new List<CharacterModifier>();
	
	private CharacterAttributeItems previousAttributes = new CharacterAttributeItems();
	
	// Update is called once per frame, updates modifiers
    void Update()
	{
		foreach (CharacterModifier modifier in Modifiers) // Iterate over attributes
		{
			if (modifier.State == ModifierState.REMOVED) {
				continue;
			} else if (modifier.DurationRemaining <= 0) { // Attribute duration finished, remove attribute
				RemoveModifier(modifier);
			} else {
				modifier.DurationRemaining -= Time.deltaTime; // Decrement duration
				modifier.OnUpdateModifier(this.characterAttributes);
			}
		}
		
		// Get dictionary of attribute changes
		Dictionary<string, object> newAttributes = getAttributes(characterAttributes, previousAttributes);
				
		// If there have been changes, emit attribute modification event with updated attributes
		if (newAttributes.Count > 0)
		{
			EventManager.TriggerEvent<CharacterAttributeChangeEvent, GameObject, Dictionary<string, object>, Dictionary<string, object>>(
				this.gameObject,
				getAttributes(previousAttributes),
				newAttributes
			);
			
			previousAttributes = (CharacterAttributeItems) characterAttributes.Clone();
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
		//Modifiers.Remove(modifier);
		modifier.OnRemoveModifier(this.characterAttributes);
		
	}
	
	// Removes a modifier from the character by modifier guid
	public void RemoveModifier(Guid modifierId) {
		CharacterModifier modifierToRemove = Modifiers.Find(modifier => modifier.Id == modifierId);
		
		if (modifierToRemove !=  null)
		{
			//Modifiers.Remove(modifierToRemove);
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
	
	// Extracts specific attribute values from the attributes class
	private Dictionary<string, object> getAttributes(CharacterAttributeItems attributes, CharacterAttributeItems compareAttributes=null) 
	{
		Dictionary<string, object> output = new Dictionary<string, object>();
		
		bool isNull = compareAttributes is null;
		
		foreach(var prop in attributes.GetType().GetProperties()) {
			// Checks if attribute is in the attribute names, and if a compare set exists, checks that they aren't identical
			
			bool isDifferent = !isNull && !compareAttributes.GetType().GetProperty(prop.Name).GetValue(compareAttributes, null).Equals(prop.GetValue(attributes, null));
			
			if (isNull || isDifferent)
			{
				//Debug.Log($"{prop.Name} - {isNull || isDifferent}, A: {compareAttributes.GetType().GetProperty(prop.Name).GetValue(compareAttributes, null)}, B: {prop.GetValue(attributes, null)}");
				output.Add(prop.Name, prop.GetValue(attributes, null));
			}
		}
		
		return output;
	}
}
