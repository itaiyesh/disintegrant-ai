using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModifier
{
	
	public Guid Id { get; set; } = Guid.NewGuid(); // Modifier instance guid
	public virtual string[] EffectedAttributes { get; set; } // List of character attributes this modifier effects
	public virtual float Duration { get; set; } = 1 / 0f; // Duration of modifier, defaults to infinite duration. Set to low value to apply modifier and remove from list.
	public float DurationRemaining;
	
	public virtual void OnUpdate(CharacterAttributeItems attributes) {
		//EventManager.TriggerEvent<CharacterAttributeEvent, CharacterAttributes>(CharacterAttributeEvent, attributes);
	}
	
	public virtual void OnAddModifier(CharacterAttributeItems attributes) {
		//EventManager.TriggerEvent<CharacterAttributeEvent, CharacterAttributes>(CharacterAttributeEvent, attributes);
	}
	
	public virtual void OnRemoveModifier(CharacterAttributeItems attributes) {
		//EventManager.TriggerEvent<CharacterAttributeEvent, CharacterAttributes>(CharacterAttributeEvent, attributes);
	}
	
}
