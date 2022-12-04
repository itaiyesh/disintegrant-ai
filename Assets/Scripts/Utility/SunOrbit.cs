using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bob object up and down
public class SunOrbit : MonoBehaviour
{
    public float speed = 5f;
    public GameObject center;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(center.transform.position, Vector3.forward, speed * Time.deltaTime);
    }
}
