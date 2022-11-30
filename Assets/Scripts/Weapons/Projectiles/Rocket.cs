using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{

    public GameObject vfxExplosion;
    public AudioClip audioClip;
    public Shockwave shockwave;

    public override void OnTriggerEnter(Collider collider)
    {
        GameObject vfxObject = Instantiate(vfxExplosion, transform.position, Quaternion.identity);
        Destroy(vfxObject, 5f); //Manually destroy VFX after some time.
        EventManager.TriggerEvent<WeaponFiredEvent, GameObject, GameObject, AudioClip, Vector3>(gameObject, gameObject, audioClip, transform.position);
        Instantiate(shockwave, transform.position, Quaternion.identity);
        base.OnTriggerEnter(collider); // Call parent method
    }
}
