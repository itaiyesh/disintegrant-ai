using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModifier : CharacterModifier
{
	public float HealthAmount { get; set; } // amount of health to modify
	public ModifierTrigger Trigger { get; set; } = ModifierTrigger.ON_ADD; // when to apply the modifier
	public bool ReverseOnRemove { get; set; } = false; // reverse the effects on remove
	
	public HealthModifier(float healthAmount, float duration = 1.0f, ModifierTrigger trigger=ModifierTrigger.ON_ADD, bool reverseOnRemove=false)
	{
		EffectedAttributes = new []{"Health"};
		Duration = duration;
		HealthAmount = healthAmount;
		Trigger = trigger;
		ReverseOnRemove = reverseOnRemove;
	}
	
	public override void OnAddModifier(CharacterAttributeItems attributes)
	{
		base.OnAddModifier(attributes);
		
		if (Trigger == ModifierTrigger.ON_ADD)
		{
			attributes.Health += HealthAmount;
		}
	}
	
	public override void OnUpdateModifier(CharacterAttributeItems attributes)
	{
		base.OnUpdateModifier(attributes);
		
		if (Trigger == ModifierTrigger.ON_UPDATE)
		{
			attributes.Health +=(HealthAmount / Duration) * Time.deltaTime;;
		}
	}
	
	public override void OnRemoveModifier(CharacterAttributeItems attributes)
	{
		base.OnRemoveModifier(attributes);
		
		if (Trigger == ModifierTrigger.ON_REMOVE)
		{
			attributes.Health += HealthAmount;
		}
		
		if (ReverseOnRemove)
		{
			attributes.Health += -HealthAmount;
		}
	}
}