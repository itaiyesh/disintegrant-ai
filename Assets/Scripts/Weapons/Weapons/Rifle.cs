using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
	public ParticleSystem particleSystem;
	
	public override void Awake()
	{
		base.Awake();
		
		particleSystem = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
	}
	
	public override void SpawnProjectile(Transform location, Vector3 direction)
	{
		particleSystem.Play();
		base.SpawnProjectile(location, direction);
	}

}
