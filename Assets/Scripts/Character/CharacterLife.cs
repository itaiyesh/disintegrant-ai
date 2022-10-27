using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class CharacterLife : MonoBehaviour
{
	
	private UnityAction<GameObject, Dictionary<string, object>, Dictionary<string, object>> characterAttributeEventListener;
	CanvasGroup gameOverMenu;
	
	public GameObject RagDoll;
	
	void Awake()
	{
		characterAttributeEventListener = new UnityAction<GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventHandler);
		gameOverMenu = GameObject.Find("Game Over Menu").GetComponent<CanvasGroup>();
	}
	
	void OnEnable()
	{
		EventManager.StartListening<CharacterAttributeChangeEvent, GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventListener);        
	}

	void OnDisable()
	{
		EventManager.StopListening<CharacterAttributeChangeEvent, GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventListener);
	}
	
	void characterAttributeEventHandler(
		GameObject triggeringPlayer,
		Dictionary<string, object> oldAttributes,
		Dictionary<string, object> newAttributes
	)
	{
		// Filter out any AI players
		if (triggeringPlayer != this.gameObject)
			return;
			
		// Player is dead, kill player
		if (newAttributes.ContainsKey("Health") && (float) newAttributes["Health"] <= 0.005f) 
		{
			Die();
			this.gameObject.GetComponent<CharacterAttributes>().characterAttributes.IsAlive = false;
			
			// Show menu if the player died
			if (this.gameObject.name == "Player") 
			{
				Time.timeScale = 0.3f;
				gameOverMenu.interactable = true;
				gameOverMenu.blocksRaycasts = true;
				gameOverMenu.alpha = 1f;
			}
		}
		
	}
	
	void Die() 
	{
		gameObject.SetActive(false);
		GameObject ragdoll = Instantiate(RagDoll, transform.position, transform.rotation);
		Destroy(ragdoll, 3);
	}
}
