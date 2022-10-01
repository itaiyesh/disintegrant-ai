using System;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierState { INITIALIZED, ADDED, UPDATED, REMOVED };
public enum ModifierTrigger { ON_ADD, ON_UPDATE, ON_REMOVE };

public class CharacterModifier
{
	public Guid Id { get; set; } = Guid.NewGuid(); // Modifier instance guid
	public ModifierState State {get; set; } = ModifierState.INITIALIZED; // Current modifier state
	public DateTime initializedAt { get; set; } = DateTime.Now; // Time of initialization
	public virtual string[] EffectedAttributes { get; set; } // List of character attributes this modifier effects (so that all modifiers effecting a certain attribute can be accesses/removed -- e.g., to clear all ill effects)
	public virtual float Duration { get; set; } = 1 / 0f; // Duration of modifier, defaults to infinite duration. Set to low value to apply modifier and remove from list.
	public float DurationRemaining; // A duration value that gets filled in with the duration when the modifier is added

	public virtual void OnUpdateModifier(CharacterAttributeItems attributes) {
		//EventManager.TriggerEvent<CharacterAttributeEvent, CharacterAttributes>(CharacterAttributeEvent, attributes);
		this.State = ModifierState.UPDATED;
	}
	
	public virtual void OnAddModifier(CharacterAttributeItems attributes) {
		//EventManager.TriggerEvent<CharacterAttributeEvent, CharacterAttributes>(CharacterAttributeEvent, attributes);
		this.State = ModifierState.ADDED;
	}
	
	public virtual void OnRemoveModifier(CharacterAttributeItems attributes) {
		//EventManager.TriggerEvent<CharacterAttributeEvent, CharacterAttributes>(CharacterAttributeEvent, attributes);
		this.State = ModifierState.REMOVED;
	}
	
}
