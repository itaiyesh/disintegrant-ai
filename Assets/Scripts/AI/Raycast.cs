using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Raycast : MonoBehaviour
{
    public NavMeshAgent agent;
    StateParams stateParams;
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f))
        {
            Debug.DrawRay(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.yellow);
        }
    }
}
