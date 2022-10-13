using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bob object up and down
public class Bob : MonoBehaviour
{
	public float speed = 5f;
	public float height = 0.1f;
	private float startY;
	
    void Start()
    {
	    startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
	    Vector3 pos = transform.position;
	    float newY = startY + height * Mathf.Sin(Time.time * speed);
	    transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
