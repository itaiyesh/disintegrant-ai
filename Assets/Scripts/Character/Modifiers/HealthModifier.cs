using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModifier : CharacterModifier
{
	public float HealthAmount { get; set; }
	
	public HealthModifier(float healthAmount, float duration = 1)
	{
		EffectedAttributes = new []{"Health"};
		Duration = duration;
		HealthAmount = healthAmount;
	}
	
	public override void OnAddModifier(CharacterAttributeItems attributes)
	{
		attributes.Health += HealthAmount;
	}
}