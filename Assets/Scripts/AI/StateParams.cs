using UnityEngine;
using UnityEngine.AI;

public class StateParams {
    public NavMeshAgent Agent;
    public WeaponController WeaponController;
    public CharacterAttributeItems Attributes;
    public GameObject Target;
	public GameObject Health;
	public GameObject Weapon;
    public Vector3 Waypoint; 
    public bool IsArmed;
    public bool IsTargetClose;
    public bool IsTargetinRange;
    public bool IsUnderAttack;
    public bool InHearingDistance;

    public bool IsGoodHealth;

	public bool IsMediumHealth;
    
	public bool IsGoodAmmo;
}