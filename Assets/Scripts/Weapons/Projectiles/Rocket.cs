using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{
	
	public GameObject vfxExplosion;
    
	public override void OnTriggerEnter(Collider collider)
	{
		GameObject vfxObject = Instantiate(vfxExplosion, transform.position, Quaternion.identity);
		Destroy(vfxObject, 5f); //Manually destroy VFX after some time.
		
		base.OnTriggerEnter(collider); // Call parent method
	}
}
