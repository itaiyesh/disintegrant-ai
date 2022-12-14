using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    public GameObject DamageFX;
    public float Damage { get; private set; }
    public float InitialSpeed { get; private set; }
    public float MaxSpeed { get; private set; }
    public float Acceleration { get; private set; }
    public float MaxDuration { get; private set; } //seconds

    public bool IsHitScan { get; private set; } //Whether or not to use hit scan as collision for this "projectile"

    // the player who fired this projectile
    public GameObject Player { get; private set; }

    private Vector3 direction;
    protected float startTime;

    private bool isTriggered = false;

    public void Init(float damage, float initialSpeed, float maxSpeed, float acceleration, float maxDuration, bool isHitScan,
        GameObject player)
    {
        Damage = damage;
        InitialSpeed = initialSpeed;
        MaxSpeed = maxSpeed;
        Acceleration = acceleration;
        MaxDuration = maxDuration;
        IsHitScan = isHitScan;
        Player = player;
    }

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        direction = transform.forward; // Set projectile direction
        bulletRigidbody.velocity = transform.forward * InitialSpeed;
        startTime = Time.fixedTime;

        if (IsHitScan)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 20f))
            {
                var c = hitInfo.transform.gameObject;
                if (c.GetComponent<CharacterAttributes>() != null)
                {
                    if (!IsOwnProjectile(c))
                    {
                        // Create damage health modifier and add to hit player
                        HealthModifier pickup = new HealthModifier(
                            healthAmount: -Damage,
                            trigger: ModifierTrigger.ON_ADD);
                        c.GetComponent<CharacterAttributes>().AddModifier(pickup); // Add the modifier to the player

                        //Trigger hit animation based on type of weapon/projectile
                        c.GetComponent<Animator>().SetTrigger("takeDamage");
                        c.GetComponent<CharacterAttributes>().Attacked(gameObject);
                        GameObject.Instantiate(DamageFX, c.transform.position, gameObject.transform.rotation);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (Acceleration > 0)
        {
            bulletRigidbody.velocity = Vector3.Slerp(bulletRigidbody.velocity, direction * MaxSpeed, Time.deltaTime * Acceleration);
        }
        if (Time.fixedTime - startTime > MaxDuration)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter(Collider c)
    {
        if (!IsHitScan)
        {
            if (c.gameObject.GetComponent<CharacterAttributes>() != null && !isTriggered)
            {
                isTriggered = true;

                if (!IsOwnProjectile(c.gameObject))
                {
                    // Create damage health modifier and add to hit player
                    HealthModifier pickup = new HealthModifier(
                        healthAmount: -Damage,
                        trigger: ModifierTrigger.ON_ADD);
                    c.gameObject.GetComponent<CharacterAttributes>().AddModifier(pickup); // Add the modifier to the player

                    //Trigger hit animation based on type of weapon/projectile
                    c.gameObject.GetComponent<Animator>().SetTrigger("takeDamage");
                    c.gameObject.GetComponent<CharacterAttributes>().Attacked(this.gameObject);
                    GameObject.Instantiate(DamageFX, gameObject.transform.position, gameObject.transform.rotation);
                }
            }
        }


        Destroy(gameObject);

    }

    /**
	 * Returns true if the projectile was fired at the player's self.
	 */
    private bool IsOwnProjectile(GameObject target)
    {
        if (Player == null) return false;
        return Player.GetInstanceID().Equals(target.GetInstanceID());
    }

}

