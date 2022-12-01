using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerLoadScene : MonoBehaviour
{
	bool isTriggered = false;
	
	public string sceneName;
	
	public void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.GetComponent<CharacterAttributes>() != null &&!isTriggered)
		{
			isTriggered = true;
			
			SceneManager.LoadScene(sceneName);
		}
	}
}


