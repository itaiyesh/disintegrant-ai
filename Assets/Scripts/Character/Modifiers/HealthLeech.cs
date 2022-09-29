using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLeech : CharacterModifier
{
	public float HealthAmount { get; set; }
	
	public HealthLeech(float healthAmount, float duration = 5)
	{
		EffectedAttributes = new []{"Health"};
		Duration = duration;
		HealthAmount = healthAmount;
	}
	
	public override void OnUpdate(CharacterAttributeItems attributes)
	{
		attributes.Health -= (HealthAmount / Duration) * Time.deltaTime; // Removes health amount uniformly over course of duration
	}
}