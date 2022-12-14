using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	public CharacterAttributeItems characterAttributes;

    [SerializeField]
    public GameObject rigLayers;
	private Animator animator;
    
	public GameObject starterWeapon;
	
    // Start is called before the first frame update
    void Awake()
    {
	    animator = rigLayers.GetComponent<Animator>();
	    characterAttributes = this.GetComponent<CharacterAttributes>().characterAttributes;
        if(!animator) { Debug.LogError("No rig animator found"); }
    }
    
    void Start()
	{
		// Add starter weapon to character
	    AddWeapon(Instantiate(starterWeapon), equip: true);
    }
    
	public void AddWeapon(GameObject weapon, bool equip = false)
	{
		weapon.SetActive(false);
		weapon.transform.position = transform.Find("RigLayers/Weapon/WeaponPlaceholder").transform.position;
		weapon.transform.rotation = transform.Find("RigLayers/Weapon/WeaponPlaceholder").transform.rotation;
		weapon.transform.parent = transform.Find("RigLayers/Weapon/WeaponPlaceholder");
		
		if (equip)
		{
			animator.SetTrigger("pull_out_" + weapon.GetComponent<Weapon>().AnimationTag);
			weapon.SetActive(true);
			
			// send a weapon swap event
			EventManager.TriggerEvent<WeaponSwapEvent, GameObject, GameObject, GameObject>(this.gameObject, null, weapon);
		}
		
		weapon.GetComponent<Weapon>().Player = this.gameObject; // Set owning player
		characterAttributes.equippedWeapons.Add(weapon);
		EventManager.TriggerEvent<WeaponAddEvent, GameObject, GameObject>(this.gameObject, weapon);
	}
	
	public void DropWeapons()
	{
		foreach(GameObject weapon in characterAttributes.equippedWeapons) 
		{
			RemoveWeapon(weapon, true);
		}
	}
	
	public void RemoveWeapon(GameObject weapon, bool dropAsCollectable = false) 
	{
		// If this is the default weapon, prevent removal
		if (characterAttributes.equippedWeapons.IndexOf(weapon) == 0)
			return;
		
		// If this is the currently equipped weapon, swap weapons first 
		if (characterAttributes.equippedWeapons.IndexOf(weapon) == characterAttributes.activeWeaponIndex)
			PreviousWeapon();
		
		characterAttributes.equippedWeapons.Remove(weapon);
		EventManager.TriggerEvent<WeaponRemoveEvent, GameObject, GameObject>(this.gameObject, weapon);
		
		if (dropAsCollectable) 
		{
			// Spawn weapon collectable
			GameObject collectable = Instantiate(
				weapon.GetComponent<Weapon>().Collectable,
				transform.position,
				Quaternion.identity
			);
			
			// Replace weapon in collectable with the weapon to remove
			Destroy(collectable.transform.GetChild(0).gameObject);
			weapon.GetComponent<Weapon>().Player = null; // Set owning player to null
			weapon.transform.parent = collectable.transform;
		}
	}

    public void NextWeapon()
    {
	    Swap(characterAttributes.equippedWeapons[mod(characterAttributes.activeWeaponIndex + 1, characterAttributes.equippedWeapons.Count)]);
    }
    
    public void PreviousWeapon()
    {
	    Swap(characterAttributes.equippedWeapons[mod(characterAttributes.activeWeaponIndex - 1, characterAttributes.equippedWeapons.Count)]);
    }
    
	public void Swap(GameObject weapon) 
	{
		if(weapon != characterAttributes.equippedWeapons[characterAttributes.activeWeaponIndex] && characterAttributes.equippedWeapons.Contains(weapon)) 
        {
			animator.SetTrigger("pull_in");
			int oldIndex = characterAttributes.activeWeaponIndex;
			characterAttributes.equippedWeapons[oldIndex].SetActive(false);
			weapon.SetActive(true);
			animator.SetTrigger("pull_out_" + weapon.GetComponent<Weapon>().AnimationTag);
			
			characterAttributes.activeWeaponIndex = characterAttributes.equippedWeapons.IndexOf(weapon);

			// send a weapon swap event
			EventManager.TriggerEvent<WeaponSwapEvent, GameObject, GameObject, GameObject>(this.gameObject, characterAttributes.equippedWeapons[oldIndex], weapon);
        }
    }

	public void Attack(Vector3 target, WeaponFireType fireType)
    {
	    GameObject equippedWeaponObject = characterAttributes.equippedWeapons[characterAttributes.activeWeaponIndex];
	    Weapon equippedWeapon = equippedWeaponObject.GetComponent<Weapon>();
	    
	    if (equippedWeapon.FireType == fireType && (equippedWeapon.LastShootTime + equippedWeapon.FireRate <= Time.time && equippedWeapon.Ammo > 0)) 
	    {
		    equippedWeapon.Attack(target);
		    animator.SetTrigger("attack");
	        
		    // If we should remove weapon when it runs out of ammo
		    if (equippedWeapon.RemoveGunOnZeroAmmo && equippedWeapon.Ammo <= 0) 
		    {
		    	RemoveWeapon(equippedWeaponObject);
		    }
    	}
    }
    
    //Mod function support for negative numbers
    //https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
    private int mod(int x, int m) {
        return (x%m + m)%m;
    }
}


