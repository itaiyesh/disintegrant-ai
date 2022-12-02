using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadProjectile : Projectile
{
    public Projectile particleProjectile;
    public int particles = 10;
    public float spreadAngle= 10f;

    public float speedVarianceRatio = 0.2f;
    private void Awake() {}

    void Start()
    {
        for (int i = 0; i < particles; i++)
        {
            Quaternion rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            Quaternion noise = Quaternion.Euler(new Vector3(Random.Range(-spreadAngle, spreadAngle),
            Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle)));
            Projectile projectileScript = GameObject.Instantiate(particleProjectile, transform.position, rotation * noise);

            projectileScript.Init(
                damage: Damage,
                initialSpeed: InitialSpeed * Random.Range(1 - speedVarianceRatio, 1 + speedVarianceRatio),
                maxSpeed: InitialSpeed,
                acceleration: Acceleration,
                maxDuration: MaxDuration,
                isHitScan: IsHitScan,
                player: Player
            );
        }

        Destroy(gameObject);

    }
    void Update(){}

}

