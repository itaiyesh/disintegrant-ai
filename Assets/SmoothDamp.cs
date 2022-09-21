using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDamp : MonoBehaviour
{
	
	public Transform target;
	public float speed = 10f;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
		Vector3 targetPosition = target.transform.position;
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
		transform.rotation = Quaternion.Euler(target.transform.rotation.eulerAngles);
    }
}
