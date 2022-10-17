using UnityEngine;
using UnityEngine.AI;

public class StateParams {
    public NavMeshAgent Agent;
    public WeaponController WeaponController;
    public CharacterAttributeItems Attributes;
    public GameObject Target;
    public bool IsArmed;
    public bool IsTargetClose;

    public bool IsGoodHealth;

    public bool IsMediumHealth;
}