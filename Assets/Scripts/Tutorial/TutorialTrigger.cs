using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
	bool isTriggered = false;
	
	public GameObject gameManager;
	public string routineName;
	
	public void OnTriggerEnter(Collider c)
	{
		if (!isTriggered)
		{
			isTriggered = true;
			
			gameManager.GetComponent<TutorialManager>().StartCoroutine(routineName);
		}
	}
}


