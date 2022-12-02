using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float MinScale = 0.0f;
    public float MaxScale = 50.0f;

    public int MaxDamage = 100;
    public float speed = 1f;

    private SphereCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        // transform.localScale = Vector3.one * MinScale;
        _collider = GetComponent<SphereCollider>();
        _collider.radius = MinScale;
    }

    // Update is called once per frame
    void Update()
    {
        _collider.radius += Time.deltaTime * speed;
        if (_collider.radius >= MaxScale)
        {
            Debug.Log("BYE");
            Destroy(gameObject);
        }
        // // transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * MaxScale, speed * Time.deltaTime);
        // // transform.localScale += Vector3.one * speed * Time.deltaTime;
        // if (transform.localScale.magnitude >= MaxScale)
        // {
        //     Destroy(gameObject);
        // }

    }
    public virtual void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<CharacterAttributes>() != null)
        {
            float distance = Vector3.Distance(transform.position, c.transform.position);
            int damage = (int)(MaxDamage * (MaxScale - distance) / MaxScale);
            // Create damage health modifier and add to hit player
            HealthModifier pickup = new HealthModifier(
                healthAmount: -damage,
                trigger: ModifierTrigger.ON_ADD);
            c.gameObject.GetComponent<CharacterAttributes>().AddModifier(pickup); // Add the modifier to the player

            //Trigger hit animation based on type of weapon/projectile
            c.gameObject.GetComponent<Animator>().SetTrigger("takeDamage");
            c.gameObject.GetComponent<CharacterAttributes>().Attacked(this.gameObject);
            // GameObject.Instantiate(DamageFX, gameObject.transform.position, gameObject.transform.rotation);
        }

    }
}
