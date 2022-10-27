using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using UnityEngine.Events;
using System.Linq;


public class HUD : MonoBehaviour
{
	private UnityAction<GameObject, Dictionary<string, object>, Dictionary<string, object>> characterAttributeEventListener;
	private UnityAction<GameObject, GameObject> weaponAddEventListener;
	private UnityAction<GameObject, GameObject> weaponRemoveEventListener;
	private UnityAction<GameObject, GameObject, AudioClip, Vector3> weaponFiredEventListener;
	private UnityAction<GameObject, GameObject, GameObject> weaponSwapEventListener;
	
	public GameObject player;
	private VisualElement healthBar;
	private VisualElement container;
	
	private VisualElement[] bars;
	private int healthBars = 10;
	
	private WeaponType lastActiveWeaponType = WeaponType.PISTOL;
	
	void Awake()
	{
		characterAttributeEventListener = new UnityAction<GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventHandler);
		weaponAddEventListener = new UnityAction<GameObject, GameObject>(weaponAddEventHandler);
		weaponRemoveEventListener = new UnityAction<GameObject, GameObject>(weaponRemoveEventHandler);
		weaponFiredEventListener = new UnityAction<GameObject, GameObject, AudioClip, Vector3>(weaponFiredEventHandler);
		weaponSwapEventListener = new UnityAction<GameObject, GameObject, GameObject>(weaponSwapEventHandler);
		
		container = GetComponent<UnityEngine.UIElements.UIDocument>().rootVisualElement.Q<VisualElement>("Container");
		healthBar = container.Q<VisualElement>("Health_Box").Q<VisualElement>("Health_Bars");
	}
	
	void OnEnable()
	{
		EventManager.StartListening<CharacterAttributeChangeEvent, GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventListener);        
		EventManager.StartListening<WeaponAddEvent, GameObject, GameObject>(weaponAddEventListener); 
		EventManager.StartListening<WeaponRemoveEvent, GameObject, GameObject>(weaponRemoveEventListener);
		EventManager.StartListening<WeaponFiredEvent, GameObject, GameObject, AudioClip, Vector3>(weaponFiredEventListener);
		EventManager.StartListening<WeaponSwapEvent, GameObject, GameObject, GameObject>(weaponSwapEventListener);
	}

	void OnDisable()
	{
		EventManager.StopListening<CharacterAttributeChangeEvent, GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventListener);
		EventManager.StopListening<WeaponAddEvent, GameObject, GameObject>(weaponAddEventListener); 
		EventManager.StopListening<WeaponRemoveEvent, GameObject, GameObject>(weaponRemoveEventListener);
		EventManager.StopListening<WeaponFiredEvent, GameObject, GameObject, AudioClip, Vector3>(weaponFiredEventListener);
		EventManager.StopListening<WeaponSwapEvent, GameObject, GameObject, GameObject>(weaponSwapEventListener);
	}

	void characterAttributeEventHandler(
		GameObject triggeringPlayer,
		Dictionary<string, object> oldAttributes,
		Dictionary<string, object> newAttributes
	)
	{
		// Filter out any AI players
		if (triggeringPlayer != player)
			return;
			
		// Update health bar
		if (newAttributes.ContainsKey("Health")) 
		{
			int healthValue = (int) Math.Ceiling((float) newAttributes["Health"]);
			AnimateBar(healthValue); // Update bars
			container.Q<VisualElement>("Health_Box").Q<Label>("Health").text = $"{healthValue} / 100"; // Update text
		}
		
	}
	
	void weaponAddEventHandler(
		GameObject triggeringPlayer,
		GameObject weapon
	)
	{
		// Filter out any AI players
		if (triggeringPlayer != player)
			return;
		
		string ammo = $"{weapon.GetComponent<Weapon>().Ammo}";
		if (weapon.GetComponent<Weapon>().Ammo == 1.0f / 0.0f) // if ammo is infinite, change to infinity symbol
		{
			ammo = "∞";
		}
		
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{weapon.GetComponent<Weapon>().WeaponType}").Q<Label>("Ammo").text = $"AMMO: {ammo}";
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{weapon.GetComponent<Weapon>().WeaponType}").style.display = DisplayStyle.Flex;
	}
	
	void weaponRemoveEventHandler(
		GameObject triggeringPlayer,
		GameObject weapon
	)
	{
		// Filter out any AI players
		if (triggeringPlayer != player)
			return;
			
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{weapon.GetComponent<Weapon>().WeaponType}").style.display = DisplayStyle.None;
	}
	
	// Get active weapon and update ammo count
	void weaponFiredEventHandler(
		GameObject triggeringPlayer,
		GameObject weapon,
		AudioClip clip,
		Vector3 position
	)
	{
		// Filter out any AI players
		if (triggeringPlayer != player)
			return;
			
		string ammo = $"{weapon.GetComponent<Weapon>().Ammo}";
		if (weapon.GetComponent<Weapon>().Ammo == 1.0f / 0.0f) // if ammo is infinite, change to infinity symbol
		{
			ammo = "∞";
		}
		
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{weapon.GetComponent<Weapon>().WeaponType}").Q<Label>("Ammo").text = $"AMMO: {ammo}";
		
	}
	
	void weaponSwapEventHandler(
		GameObject triggeringPlayer,
		GameObject oldWeapon,
		GameObject newWeapon
	) 
	{
		// Filter out any AI players
		if (triggeringPlayer != player)
			return;
			
		// Unselect old weapon
		if (oldWeapon != null)
		{
			container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{oldWeapon.GetComponent<Weapon>().WeaponType}").style.borderTopWidth = new StyleFloat(0f);
			container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{oldWeapon.GetComponent<Weapon>().WeaponType}").style.borderRightWidth = new StyleFloat(0f);
			container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{oldWeapon.GetComponent<Weapon>().WeaponType}").style.borderBottomWidth = new StyleFloat(0f);
			container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{oldWeapon.GetComponent<Weapon>().WeaponType}").style.borderLeftWidth = new StyleFloat(0f);
			container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{oldWeapon.GetComponent<Weapon>().WeaponType}").Q<VisualElement>("Icon").style.unityBackgroundImageTintColor = new StyleColor(new Color(255, 255, 255, 128));
			container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{oldWeapon.GetComponent<Weapon>().WeaponType}").Q<Label>("Ammo").style.color = new StyleColor(new Color(0, 0, 0, 255));
		}
		
		// Select new weapon
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{newWeapon.GetComponent<Weapon>().WeaponType}").style.borderTopWidth = new StyleFloat(1.5f);
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{newWeapon.GetComponent<Weapon>().WeaponType}").style.borderRightWidth = new StyleFloat(1.5f);
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{newWeapon.GetComponent<Weapon>().WeaponType}").style.borderBottomWidth = new StyleFloat(1.5f);
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{newWeapon.GetComponent<Weapon>().WeaponType}").style.borderLeftWidth = new StyleFloat(1.5f);
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{newWeapon.GetComponent<Weapon>().WeaponType}").Q<VisualElement>("Icon").style.unityBackgroundImageTintColor = new StyleColor(new Color(255, 255, 255, 255));
		container.Q<VisualElement>("Weapons_Box").Q<VisualElement>($"Weapon_{newWeapon.GetComponent<Weapon>().WeaponType}").Q<Label>("Ammo").style.color = new StyleColor(new Color(118, 110, 110, 255));
		
	}

	
	public void AnimateBar(int health)
	{
		// TODO: This is problably not amazing for perforamnce. Better to use a health update event.
		int newHealthBars = (int) Math.Ceiling(player.GetComponent<CharacterAttributes>().characterAttributes.Health / 10);
		
		if (newHealthBars != healthBars) 
		{
			int i = 0;
			foreach(VisualElement bar in healthBar.Children())
			{
				if (i <= newHealthBars)
				{
					bar.style.visibility = Visibility.Visible;
				} else {
					bar.style.visibility = Visibility.Hidden;
				}
				i += 1;
			}
			
		}
	}
	
	//private void LateUpdate()
	//{
	//	// Update health
	//	float healthValue = Math.Ceiling(player.GetComponent<CharacterAttributes>().characterAttributes.Health);
	//	if (lastHealth != healthValue)
	//	{
			
			
	//		// Update health bars
	//		AnimateBar();
			
	//		lastHealth = healthValue;
	//	}
		
	//	// Update weapons
		
	//}
    
	
}
