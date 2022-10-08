using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunFire : MonoBehaviour
{
    public ParticleSystem system;

    private void Start()
    {
        system.enableEmission = false;
 
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()

    {
        while (Input.GetMouseButton(0))
        {
            system.enableEmission = true;
            system.Play();
            yield return null;
        }
        system.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
