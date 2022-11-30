using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Events;

public enum WeaponType
{
    PISTOL,
    RIFLE,
    SHOTGUN,
    RPG
}

public enum WeaponFireType
{
    SINGLE,
    RAPID
}

// Weapon class, contains all details relevant to a weapon implementation
public class Weapon : MonoBehaviour
{
    public string Name;

    public WeaponType WeaponType;
    public WeaponFireType FireType;

    public GameObject Player;

    public GameObject Projectile;
    public AudioClip FireSound;
    public GameObject Collectable;
    public string AnimationTag;

    // Weapon parameters
    public float FireRate = 0.3f; // seconds between shots
    public float Ammo = 1.0f / 0.0f;

    // Projectile parameters
    public float Damage = 1f;
    public float InitialSpeed = 0f;
    public float MaxSpeed = 50f;
    public float Acceleration = 3f;
    public float MaxDuration = 10f; //seconds

    public float LastShootTime;
    public bool RemoveGunOnZeroAmmo = false;

    public virtual void Awake()
    {
        LastShootTime = Time.time;
    }

    public virtual void Attack(Transform target)
    {

        LastShootTime = Time.time;

        // Get bullet spawn position
        Transform bulletSpawnPosition = transform.Find($"BulletSpawnPosition").transform;

        // Calculate aim direction
        Vector3 aimDirection = (target.position - bulletSpawnPosition.position).normalized;
        //aimDirection.y = 0; // Constraining to horizontal aiming only

        Ammo -= 1;

        // Spawn projectile and set stats
        SpawnProjectile(bulletSpawnPosition, aimDirection);

        // Play fire sound & trigger fire event
        TriggerEvent(FireSound, bulletSpawnPosition.position);

    }

    public virtual void SpawnProjectile(Transform location, Vector3 direction)
    {
        // Instantiate projectile
        GameObject projectile = GameObject.Instantiate(Projectile, location.position, Quaternion.LookRotation(direction, Vector3.up));
        Projectile projectileScript = GameObject.FindObjectOfType<Projectile>(); // Get projectile script

        // Pass stats to projectile
        projectileScript.Init(
            damage: Damage,
            initialSpeed: InitialSpeed,
            maxSpeed: MaxSpeed,
            acceleration: Acceleration,
            maxDuration: MaxDuration,
            player: Player
        );
    }

    public void TriggerEvent(AudioClip audioClip, Vector3 position)
    {
        EventManager.TriggerEvent<WeaponFiredEvent, GameObject, GameObject, AudioClip, Vector3>(Player, this.gameObject, audioClip, position);
    }
}
