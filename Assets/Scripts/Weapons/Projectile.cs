﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
	
	[HideInInspector] public float Damage;
	[HideInInspector] public float InitialSpeed;
	[HideInInspector] public float MaxSpeed;
	[HideInInspector] public float Acceleration;
	[HideInInspector] public float MaxDuration; //seconds
    
    private Vector3 direction;
	private float startTime;
    
	private bool isTriggered = false;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
	    direction = transform.forward; // Set projectile direction
        bulletRigidbody.velocity = transform.forward * InitialSpeed;
        startTime = Time.fixedTime;
    }

    void Update()
    {
        bulletRigidbody.velocity = Vector3.Slerp(bulletRigidbody.velocity, direction * MaxSpeed, Time.deltaTime * Acceleration);
        if(Time.fixedTime - startTime > MaxDuration)
        {
            Destroy(gameObject);
        }
    }

	public virtual void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.GetComponent<CharacterAttributes>() != null && !isTriggered)
		{
			isTriggered = true;
			
			// Create damage health modifier and add to hit player
			HealthModifier pickup = new HealthModifier(
				healthAmount: -Damage,
				trigger: ModifierTrigger.ON_ADD);
			c.gameObject.GetComponent<CharacterAttributes>().AddModifier(pickup); // Add the modifier to the player

			Destroy(gameObject);
		}
    }

}
