using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Animator))]
public class MachineGunFire : MonoBehaviour
{
    public ParticleSystem system;

    public GameObject gun;

    // trail implementation adapted from https://youtu.be/cI3E7_f74MA
    [SerializeField]
    private bool bulletSpread = true;
    [SerializeField]
    private Vector3 bulletSpreadVariance = new Vector3(0.05f, 0.05f, 0.05f);
    [SerializeField]
    // shootingSystem may take the muzzle fire, impactParticleSystem takes impact FX
    //private ParticleSystem shootingSystem;
    //[SerializeField]
    //private ParticleSystem impactParticleSystem;
    //[SerializeField]
    private TrailRenderer bulletTrail;
    [SerializeField]
    private Transform bulletSpawnPoint;
    private float shootDelay = 0.05f;
    private float lastShootTime;

    private LayerMask mask;

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
            if ((lastShootTime + shootDelay) < Time.time) { 
                Vector3 direction = GetDirection();
                if (Physics.Raycast(bulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue))
                {
                    TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));
                    lastShootTime = Time.time;
                }
            }
            yield return null;
	        //}
          
        system.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private Vector3 GetDirection()
    {

        Vector3 direction = gun.transform.forward;
        if (bulletSpread)
        {
            direction += new Vector3(
                Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
                );
            direction.Normalize();

        }
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;
        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = Hit.point;
        // Instantiate(impactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));

        Destroy(Trail.gameObject, Trail.time);
    }
}
