using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;


public class EnemyAI : MonoBehaviour
{
    public FSM fsm = new FSM();
    public NavMeshAgent agent;
    private Animator animator;

    //State Machine variables:
    public float PlayerDist = 10.0f; // value to determine if player is close to agent
    public float goodHealth = 0.66f; // threshold between medium and good agent health
    public float mediumHealth = 0.33f; // threshold between bad and medium agent health
    private StateParams stateParams;

    private bool IsTargetClose(Vector3 agentPos, Vector3 AIPos)
    {
        return Vector3.Distance(agentPos, AIPos) < PlayerDist;
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
        //Initial State:
        fsm.Switch(Idle.Instance);
    }

    // Update is called once per frame
    void Update()
    {
        //Collect common FSM variables
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int minIndex = -1;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == gameObject) { continue; }
            float distance = Vector3.Distance(players[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }
        stateParams.Target = minIndex > -1 ? players[minIndex] : null;
        stateParams.IsTargetClose = stateParams.Target != null && IsTargetClose(stateParams.Target.transform.position, agent.transform.position);
        stateParams.IsArmed = IsArmed(stateParams.Attributes);
        stateParams.IsGoodHealth = stateParams.Attributes.Health > goodHealth;
        stateParams.IsMediumHealth = stateParams.Attributes.Health > mediumHealth;

        //Execute current state
        fsm.Execute(stateParams);

        //Update animator
        animator.SetFloat("vely", agent.velocity.magnitude / agent.speed);
    }
    // public enum AIState
    // {
    //     Wander,
    //     Attack,
    //     CollectGun,
    //     CollectHealth,
    //     SeekCover
    // };
    ////////////////////
    //state parameters:
    // got gun: true, false -> Would need to get more complicated to choose which gun to attack with based on ammo and available gun
    // health: bad (< 33 %), medium (33-66 %), good (>66 %)
    // player closeness: close, far -> needs decision how to implement, e.g. player in a specific room//triggered a trigger, or actual distance
    ////////////////////

    //////State arbitration below////////////
    // if player far away, health good and got gun: idle

    // if (!isTargetClose && agentAttribs.Health > mediumHealth && isArmed)
    // {
    //     // aiState = AIState.Idle;
    // }

    // // if player far away, health bad and got gun/ got no gun: go to next health
    // // if player close and health bad and got gun/ got no gun: go to next health
    // if (!isTargetClose && (agentAttribs.Health < mediumHealth) && (isArmed || !isArmed))
    // {
    //     // aiState = AIState.CollectHealth;
    // }

    // // if player far away, health good / medium and got no gun: go to next gun
    // // if player close and health good and got no gun: go to next gun
    // if ((!isTargetClose && (agentAttribs.Health > mediumHealth) && (isArmed || !isArmed)) ||
    //     (isTargetClose && (agentAttribs.Health > mediumHealth) && (!isArmed)))
    // {
    //     // aiState = AIState.CollectGun;
    // }


    // // if player close and health good and got gun: attack
    // else if (isTargetClose && agentAttribs.Health >= mediumHealth && isArmed)
    // {
    //     // aiState = AIState.Attack;
    // }

    // // if player close and health medium and got gun: seekCover
    // else if (isTargetClose && agentAttribs.Health >= mediumHealth &&
    //     agentAttribs.Health < goodHealth && isArmed)
    // {
    //     // aiState = AIState.Attack;
    // }

    // Debug.Log("Agent is in state " + aiState + ".");


    // switch (aiState)
    // {
    //     case AIState.Idle:
    //         // select some random points in agents vincinity, check that they are on the NavMesh and let him roam around
    //         break;

    //     case AIState.Attack:
    //         // let agent approach player up to some distance, open fire
    //         break;

    // }
}
