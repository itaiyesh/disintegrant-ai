using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using Events;

public class EnemyAI : MonoBehaviour
{
    public FSM fsm = new FSM();
    public NavMeshAgent agent;
    private Animator animator;

    //State Machine variables:
    private float PlayerDist = 20.0f; // value to determine if player is close to agent
    private int goodHealth = 90; // threshold between medium and good agent health
    private int mediumHealth = 50; // threshold between bad and medium agent health
    public float chaseAttackRatio = 0.5f;
    private StateParams stateParams;
    public GameObject _target;
    //private UnityAction<AudioClip, Vector3> weaponFiredEventListener; 

    private bool IsTargetClose(Vector3 agentPos, Vector3 AIPos)
    {
        // Debug.Log("Chase - IsTargetClose ?: " + (Vector3.Distance(agentPos, AIPos) < PlayerDist) + "; Distance= " +
        //     Vector3.Distance(agentPos, AIPos) + " PlayerDist= " + (PlayerDist));
        return Vector3.Distance(agentPos, AIPos) < PlayerDist;
    }

    private bool IsTargetinRange(Vector3 agentPos, Vector3 AIPos)
    {
        // target is in shooting range
        // Debug.Log("Attack - IsTargetinRange ?: " + (Vector3.Distance(agentPos, AIPos) < PlayerDist * 0.6) + "; Distance= " +
        //     Vector3.Distance(agentPos, AIPos) + " PlayerDist*0.6= " + (PlayerDist * chaseAttackRatio));

        return Vector3.Distance(agentPos, AIPos) < PlayerDist * 0.6;
    }

    private bool InHearingDistance(Vector3 agentPos, Vector3 AIPos)
    {
        // target is in hearing distance when firing a shot
        return Vector3.Distance(agentPos, AIPos) < PlayerDist * 2;
    }

    private void IsUnderAttack(Vector3 agentPos, Vector3 AIPos)
    {
        // target is in hearing distance when firing a shot
        stateParams.Attributes.IsUnderAttack = true;
    }

    private bool IsArmed(CharacterAttributeItems attribs)
    {
        return attribs.equippedWeapons.Count > 1;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("No Animator set on gameobject");
        }
        agent = GetComponent<NavMeshAgent>();
        if (!agent)
        {
            Debug.LogError("No agent set on gameobject");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stateParams = new StateParams();
        stateParams.Agent = agent;
        stateParams.Attributes = GetComponent<CharacterAttributes>().characterAttributes;
        stateParams.WeaponController = GetComponent<WeaponController>();
        stateParams.AIAgressiveness = FindObjectOfType<GameManager>().AIAgressiveness;
        stateParams.AIAimSpread = FindObjectOfType<GameManager>().AIAimSpread;

        fsm.Agent = agent;
        fsm.VoicePack = stateParams.Attributes.VoicePack;
        //Initial State:
        fsm.Switch(Idle.Instance);
    }


    // Update is called once per frame
    void Update()
    {
        //Collect common FSM variables
        //Find closest player to fight
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] AIs = GameObject.FindGameObjectsWithTag("AI");
        GameObject[] players = player.Concat(AIs).ToArray();

        int minIndex = -1;
        float minDistance = Mathf.Infinity;
        Vector3 playersCenterOfMass = Vector3.zero;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == gameObject) { continue; }
            float w = i == 0 ? (1 - stateParams.AIAgressiveness) : stateParams.AIAgressiveness;
            float distance = w * Vector3.Distance(players[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }

            playersCenterOfMass += players[i].transform.position;
        }
        if (players.Length > 0) { playersCenterOfMass /= players.Length; } //TODO:Else?
        //find closest health pack to pick up
        GameObject[] health = GameObject.FindGameObjectsWithTag("HealthCollectable");
        int minIndexHealth = -1;
        float minDistanceHealth = Mathf.Infinity;
        for (int i = 0; i < health.Length; i++)
        {
            float distance = Vector3.Distance(health[i].transform.position, transform.position);
            if (distance < minDistanceHealth)
            {
                minDistanceHealth = distance;
                minIndexHealth = i;
            }
        }

        //find closest weapon pack to pick up
        List<GameObject> equippedWeapons = stateParams.Attributes.equippedWeapons != null ? stateParams.Attributes.equippedWeapons : new List<GameObject>();
        List<GameObject> loadedWeapons = equippedWeapons.FindAll(weapon => weapon.GetComponent<Weapon>().Ammo > 0);
        HashSet<WeaponType> loadedWeaponTypes = new HashSet<WeaponType>(loadedWeapons.Select(weapon => weapon.GetComponent<Weapon>().WeaponType));

        GameObject[] weapon = GameObject.FindGameObjectsWithTag("WeaponCollectable");
        int minIndexWeapon = -1;
        float minDistanceWeapon = Mathf.Infinity;
        for (int i = 0; i < weapon.Length; i++)
        {
            if (loadedWeaponTypes.Contains(weapon[i].GetComponentInChildren<Weapon>().WeaponType)) { continue; }
            float distance = Vector3.Distance(weapon[i].transform.position, transform.position);
            if (distance < minDistanceWeapon)
            {
                minDistanceWeapon = distance;
                minIndexWeapon = i;
            }
        }

        stateParams.NearestPlayer = minIndex > -1 ? players[minIndex] : null;
        stateParams.Health = minIndexHealth > -1 ? health[minIndexHealth] : null;
        stateParams.Weapon = minIndexWeapon > -1 ? weapon[minIndexWeapon] : null;
        stateParams.IsTargetClose = stateParams.NearestPlayer != null && IsTargetClose(stateParams.NearestPlayer.transform.position, agent.transform.position);
        stateParams.IsTargetinRange = stateParams.NearestPlayer != null && IsTargetinRange(stateParams.NearestPlayer.transform.position, agent.transform.position);
        stateParams.IsArmed = IsArmed(stateParams.Attributes);
        stateParams.IsGoodHealth = stateParams.Attributes.Health > goodHealth;
        stateParams.IsMediumHealth = stateParams.Attributes.Health > mediumHealth;
        stateParams.PlayersCenterOfMass = playersCenterOfMass;
        stateParams.LoadedWeapons = loadedWeapons;
        //stateParams.InHearingDistance = InHearingDistance(stateParams.Target.transform.position, agent.transform.position);

        //Execute current state
        fsm.Execute(stateParams);

        //Update animator
        animator.SetFloat("vely", Vector3.Dot(agent.velocity, agent.transform.forward) / agent.speed);
        animator.SetFloat("velx", Vector3.Dot(agent.velocity, agent.transform.right) / agent.speed);

        //Draw Raycast in shooting direction
        if (Physics.Raycast(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f))
        {
            Debug.DrawRay(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.yellow);
        }

        //Set sphere to current goal of agent
        _target.transform.position = stateParams.Agent.destination;

    }
}
