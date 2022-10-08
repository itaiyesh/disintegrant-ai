using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [SerializeField]
    public WeaponConfig[] weapons; 

    // public List<WeaponConfig> weapons = new List<WeaponConfig>(); 
    public List<WeaponConfig> equippedWeapons = new List<WeaponConfig>(); //make sure to sort

    public int activeWeaponIndex = 0;

    [SerializeField]
    public GameObject rigLayers;
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = rigLayers.GetComponent<Animator>();
        if(!animator) { Debug.LogError("No rig animator found"); }

        //Debug - give all weapons
        equippedWeapons.Clear();
        foreach(WeaponConfig weapon in weapons)
        {
            equippedWeapons.Add(weapon);
        } 
    }
    
    void Start()
    {
        var weapon = equippedWeapons[0]; 
        animator.SetTrigger("pull_out_"+weapon.tag);
        activeWeaponIndex = equippedWeapons.IndexOf(weapon);
    }

    public void NextWeapon()
    {
        Swap(equippedWeapons[mod(activeWeaponIndex + 1, equippedWeapons.Count)]);
    }
    public void PreviousWeapon()
    {
        Swap(equippedWeapons[mod(activeWeaponIndex - 1, equippedWeapons.Count)]);
    }
    private void Swap(WeaponConfig weapon) 
    {
        if(weapon != equippedWeapons[activeWeaponIndex] && equippedWeapons.Contains(weapon)) 
        {
            animator.SetTrigger("pull_in");
            animator.SetTrigger("pull_out_" + weapon.tag);
            activeWeaponIndex = equippedWeapons.IndexOf(weapon);
            
            // send a weapon change sound event.
            EventManager.TriggerEvent<WeaponSwapEvent, Vector3>(gameObject.transform.position);
        }
    }

    public void Attack(Transform target)
    {
        equippedWeapons[activeWeaponIndex].Attack(target);
        animator.SetTrigger("attack");
    }
    //Mod function support for negative numbers
    //https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
    private int mod(int x, int m) {
        return (x%m + m)%m;
    }
}


