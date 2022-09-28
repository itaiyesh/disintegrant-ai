using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    public GameObject vfxExplosion;
    public float initialSpeed = 0f;
    public float maxSpeed = 50f;
    public float acceleration = 3f;
    public float maxDuration = 10f; //seconds
    private Vector3 direction;
    private float startTime;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
        bulletRigidbody.velocity = transform.forward * initialSpeed;
        startTime = Time.fixedTime;
    }

    void Update()
    {
        bulletRigidbody.velocity = Vector3.Slerp(bulletRigidbody.velocity, direction * maxSpeed, Time.deltaTime * acceleration);
        if(Time.fixedTime - startTime > maxDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject vfxObject = Instantiate(vfxExplosion, transform.position, Quaternion.identity);
        Destroy(vfxObject, 5f); //Manually destroy VFX after some time.
        Destroy(gameObject);
    }

}
