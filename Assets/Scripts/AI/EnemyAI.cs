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
    public float PlayerDist = 15.0f; // value to determine if player is close to agent
    public float goodHealth = 0.66f; // threshold between medium and good agent health
    public float mediumHealth = 0.33f; // threshold between bad and medium agent health
    public float chaseAttackRatio = 0.6f;
    private StateParams stateParams;
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

        return Vector3.Distance(agentPos, AIPos) < PlayerDist*0.6;
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
        //Initial State:
        fsm.Switch(Idle.Instance);
    }


    // Update is called once per frame
    void Update()
    {
        //Collect common FSM variables
        //Find closest player to fight
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

        //find closest health pack to pick up
        GameObject[] health = GameObject.FindGameObjectsWithTag("health");
        int minIndexHealth = -1;
        float minDistanceHealth = Mathf.Infinity;
        for (int i = 0; i < health.Length; i++)
        {
            if (health[i] == gameObject) { continue; }
            float distance = Vector3.Distance(health[i].transform.position, transform.position);
            if (distance < minDistanceHealth)
            {
                minDistanceHealth = distance;
                minIndexHealth = i;
            }
        }

        stateParams.Target = minIndex > -1 ? players[minIndex] : null;
        stateParams.Health = minIndexHealth > -1 ? health[minIndexHealth] : null;
        stateParams.IsTargetClose = stateParams.Target != null && IsTargetClose(stateParams.Target.transform.position, agent.transform.position);
        stateParams.IsTargetinRange = stateParams.Target != null && IsTargetinRange(stateParams.Target.transform.position, agent.transform.position);
        stateParams.IsArmed = IsArmed(stateParams.Attributes);
        stateParams.IsGoodHealth = stateParams.Attributes.Health > goodHealth;
        stateParams.IsMediumHealth = stateParams.Attributes.Health > mediumHealth;
        //stateParams.InHearingDistance = InHearingDistance(stateParams.Target.transform.position, agent.transform.position);

        //Execute current state
        fsm.Execute(stateParams);

        //Update animator
        animator.SetFloat("vely", agent.velocity.magnitude / agent.speed);
    }
}
