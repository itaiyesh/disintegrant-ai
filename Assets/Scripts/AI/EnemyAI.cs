using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Attack,
        CollectGuns,
        CollectHealth,
        SeekCover
    };

    public AIState aiState;


    public NavMeshAgent agent;
    public GameObject player;

    public Animator anim;

    //get the agent attributes
    //var agentAttribs = agent.GetComponent<CharacterAttributeItems>();

    //State Machine variables:
    public float PlayerDist = 5.0f; // value to determine if player is close to agent
    public float goodHealth = 0.66f; // threshold between medium and good agent health
    public float mediumHealth = 0.33f; // threshold between bad and medium agent health


    public int noWaypoints = 5;
    public double reachDist = 0.5;
    public GameObject[] waypoints;
    int currWaypoint = -1;

    private Vector3 agentPos;
    private Vector3 wpPos;
    private Vector3 relVec;
    private float displacement;
    private Vector3 prevWpLoc;

    public GameObject _Sphere;

    private NavMeshHit hit;
    private bool blocked = false;

    private void setNextWayPoint(int currWP, GameObject[] waypoints)
    {
        if ((currWP + 1) == waypoints.Length)
        {
            currWaypoint = -1;
            currWP = -1;
        }
        currWP++;
        agent.SetDestination(waypoints[currWP].transform.position);
        currWaypoint++;
    }

    private void setPredictionPoint(Vector3 WaypointPosition)
    {

        //_Sphere.transform.localScale = WaypointPosition;
        _Sphere.transform.position = WaypointPosition;
    }

    // returns a boolean if player is close or not
    private bool isPlayerClose(Vector3 agentPos, Vector3 AIPos)
    {
        if ((agentPos - AIPos).magnitude < PlayerDist)
        {
            return true;
        }
        return false;
    }

    private bool isArmed(CharacterAttributeItems attribs)
    {
        if (attribs.equippedWeapons.Count == 1)
            { return false; }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = 9.5f;
        aiState = AIState.Idle;

        setNextWayPoint(-1, waypoints);

        prevWpLoc = waypoints[5].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////
        //state parameters:
        // got gun: true, false -> Would need to get more complicated to choose which gun to attack with based on ammo and available gun
        // health: bad (< 33 %), medium (33-66 %), good (>66 %)
        // player closeness: close, far -> needs decision how to implement, e.g. player in a specific room//triggered a trigger, or actual distance
        ////////////////////

        //var agentAttribs = agent.GetComponent<CharacterAttributeItems>();
        var agentAttribs = player.gameObject.GetComponent<CharacterAttributes>().characterAttributes;
        bool PlayerIsClose = isPlayerClose(player.transform.position, agent.transform.position);
        bool armed = isArmed(agentAttribs);

        //////State arbitration below////////////
        // if player far away, health good and got gun: idle

        if (!PlayerIsClose && agentAttribs.Health >= mediumHealth && armed )
        {
            aiState = AIState.Idle;
        }

        // if player far away, health bad and got gun/ got no gun: go to next health

        // if player far away, health good / medium and got no gun: go to next gun


        // if player close and health good and got no gun: go to next gun

        // if player close and health bad and got gun/ got no gun: go to next health

        // if player close and health good and got gun: attack
        else if (PlayerIsClose && agentAttribs.Health >= mediumHealth && armed)
        {
            aiState = AIState.Attack;
        }

        // if player close and health medium and got gun: seekCover


        Debug.Log("Agent is in state " + aiState + ".");


        switch (aiState)
        {
            case AIState.Idle:
                // select some random points in agents vincinity, check that they are on the NavMesh and let him roam around
                break;

            case AIState.Attack:
                // let agent approach player up to some distance, open fire
                break;

        }
    }
}
