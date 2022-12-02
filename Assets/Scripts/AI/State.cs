using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

class Utils
{
    public static void Attack(StateParams stateParams)
    {
        // List<GameObject> equippedWeapons = stateParams.Attributes.equippedWeapons;
        // int bestLoadedWeaponIndex = equippedWeapons.FindLastIndex(weapon => weapon.GetComponent<Weapon>().Ammo > 0);
        // if (bestLoadedWeaponIndex != stateParams.Attributes.activeWeaponIndex)
        // {
        //     stateParams.WeaponController.Swap(equippedWeapons[bestLoadedWeaponIndex]);
        // }
        // GameObject equippedWeapon = equippedWeapons[stateParams.Attributes.activeWeaponIndex];

        // float dist = (movingWaypoint.transform.position - navMeshAgent.transform.position).magnitude;
        // float lookAheadTime = Mathf.Clamp(dist / navMeshAgent.speed, 0, 4);
        // Vector3 futureTarget = movingWaypoint.transform.position + lookAheadTime * velocityReporter.velocity;
        // if (NavMesh.Raycast(movingWaypoint.transform.position, futureTarget, out hit, NavMesh.AllAreas))
        // {
        //     //Raycast is blocked 
        //     Vector3 clampedTarget = hit.position + (movingWaypoint.transform.position - hit.position).normalized;
        //     destinationTracker.transform.position = clampedTarget;
        //     navMeshAgent.SetDestination(clampedTarget);
        // }
        // else
        // {
        //     destinationTracker.transform.position = futureTarget;
        //     navMeshAgent.SetDestination(futureTarget);
        // }

        //Assuming last weapon is always the best
        stateParams.WeaponController.Swap(stateParams.LoadedWeapons[stateParams.LoadedWeapons.Count - 1]);
        GameObject equippedWeapon = stateParams.Attributes.equippedWeapons[stateParams.Attributes.activeWeaponIndex];
        Weapon equippedWeaponObject = equippedWeapon.GetComponent<Weapon>();

        // GameObject nearestPlayer = stateParams.NearestPlayer;
        // float targetDistance = Vector3.Distance(nearestPlayer.transform.position, stateParams.Agent.transform.position);
        // float lookAheadTime = targetDistance / equippedWeaponObject.MaxSpeed;
        // VelocityReporter reporter = nearestPlayer.GetComponent<VelocityReporter>();
        // if (reporter == null)
        // {
        //     Debug.LogError("NO REPORTER");
        //     return;
        // }
        // Vector3 futureTarget = nearestPlayer.transform.position + lookAheadTime * reporter.velocity;// + new Vector3(0f, 1f, 0f);
        // Vector3 currentTargetDirection = nearestPlayer.transform.position - stateParams.Agent.transform.position;
        // Vector3 futureTargetDirection = futureTarget - stateParams.Agent.transform.position;
        // stateParams.Agent.transform.rotation = Quaternion.Lerp(stateParams.Agent.transform.rotation,
        //  Quaternion.LookRotation(futureTargetDirection), Time.deltaTime * 20f);
        // // stateParams.Agent.transform.LookAt(futureTargetDirection);

        // stateParams.Agent.updateRotation = true;
        // // bool blocked = Physics.Raycast(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f);
        // // Debug.DrawRay(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);
        // bool blocked = Physics.Raycast(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f);
        // if (hitInfo.transform != null && Vector3.Distance(hitInfo.transform.position, stateParams.Agent.transform.position) < targetDistance)
        // {
        //     //A closer object than target is blocking our view
        //     return;
        // }
        // stateParams.WeaponController.Attack(futureTarget, equippedWeapon.GetComponent<Weapon>().FireType);



        Vector3 targetDirection = stateParams.NearestPlayer.transform.position - stateParams.Agent.transform.position;
        stateParams.Agent.transform.rotation = Quaternion.Lerp(stateParams.Agent.transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 45f);
        stateParams.Agent.updateRotation = true;
        bool blocked = Physics.Raycast(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f);
        Debug.DrawRay(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);
        if (hitInfo.transform != null && (hitInfo.transform.CompareTag("Player") || hitInfo.transform.CompareTag("AI")))
        {
            stateParams.WeaponController.Attack(stateParams.NearestPlayer.transform.position, equippedWeapon.GetComponent<Weapon>().FireType);

        }
    }
}
//State classes
public sealed class Idle : State
{
    public GameObject _target;
    public static readonly Idle Instance = new Idle();

    bool isWayPointvalid = false;
    private bool blocked = true;
    private NavMeshHit hit;

    public override void Execute(FSM fsm, StateParams stateParams)
    {
        List<GameObject> loadedWeapons = stateParams.LoadedWeapons;

        if (stateParams.IsMediumHealth && stateParams.Weapon != null && loadedWeapons.Count <= 1 && !stateParams.Attributes.IsUnderAttack)
        {
            fsm.Switch(CollectWeapon.Instance);
        }
        else if (stateParams.IsMediumHealth && stateParams.IsTargetinRange)
        {
            fsm.Switch(Combat.Instance);
        }
        else if (stateParams.IsMediumHealth && stateParams.IsTargetClose)
        {
            fsm.Switch(Chase.Instance);
        }
        else if (!stateParams.IsGoodHealth && stateParams.Health != null)
        {
            fsm.Switch(Heal.Instance);
        }
        else if (stateParams.Weapon != null && loadedWeapons.Count <= 2)
        {
            //Get more weapons, why not
            fsm.Switch(CollectWeapon.Instance);
        }
        else
        {
            //Select a nearby accessible waypoint to wander towards

            while (!isWayPointvalid)//&& stateParams.Agent.remainingDistance < 0.5)
            {
                //Wander towards nearest target
                NavMeshHit hit;
                if (NavMesh.SamplePosition(stateParams.NearestPlayer.transform.position, out hit, 10f, NavMesh.AllAreas))
                {
                    stateParams.Agent.SetDestination(hit.position);
                    isWayPointvalid = true;
                }


                // NavMeshHit hit;
                // if (NavMesh.SamplePosition(stateParams.PlayersCenterOfMass, out hit, 30f, NavMesh.AllAreas))
                // {
                //     var offset = Random.insideUnitCircle * 20;
                //     var newWaypoint = hit.position + new Vector3(offset.x, 0.0f, offset.y);
                //     if (NavMesh.SamplePosition(newWaypoint, out hit, 20f, NavMesh.AllAreas))
                //     {
                //         stateParams.Agent.SetDestination(hit.position);
                //         isWayPointvalid = true;
                //     }
                // }

                // var offset = Random.insideUnitCircle * 10f;
                // var newWaypoint = stateParams.Agent.transform.position + new Vector3(offset.x, 0.0f, offset.y);
                // NavMeshHit hit;
                // if (NavMesh.SamplePosition(newWaypoint, out hit, 10f, NavMesh.AllAreas))
                // {
                //     stateParams.Agent.SetDestination(hit.position);
                //     isWayPointvalid = true;
                // }


                // var centerOfMassDirection = (stateParams.PlayersCenterOfMass - stateParams.Agent.transform.position).normalized;
                // var randomOffset = Random.insideUnitCircle * 20f;
                // Vector3 randomVectorOffset = new Vector3(randomOffset.x, 0.0f, randomOffset.y);
                // var newWaypoint = stateParams.Agent.transform.position + centerOfMassDirection * 20f + randomVectorOffset;
                // NavMeshHit hit;
                // if (NavMesh.SamplePosition(newWaypoint, out hit, 50f, NavMesh.AllAreas))
                // {
                //     stateParams.Agent.SetDestination(hit.position);
                //     isWayPointvalid = true;
                // }

            }

            if (stateParams.Agent.remainingDistance < 0.5 || stateParams.Agent.isStopped)
            {
                isWayPointvalid = false;
            }

        }
    }
}

public sealed class Combat : State
{
    public static readonly Combat Instance = new Combat();

    public float PositionUpdateFrequency = 2f;
    public float AttackRadius = 5f;
    private float lastUpdateTime = 0f;
    private bool blocked = true;
    private NavMeshHit hit;
    public override void Execute(FSM fsm, StateParams stateParams)
    {
        if (stateParams.IsMediumHealth && stateParams.IsTargetinRange)
        {
            Utils.Attack(stateParams);

            if (stateParams.Agent.remainingDistance < 0.5f || Time.fixedTime - lastUpdateTime > PositionUpdateFrequency)
            {
                // Debug.Log("Time check: " + (Time.fixedTime - lastUpdateTime));
                bool newDestValid = false;
                Vector3 attackPosition = stateParams.NearestPlayer.transform.position;
                while (newDestValid == false)
                {
                    Vector2 rand = Random.insideUnitCircle * AttackRadius;
                    attackPosition += new Vector3(rand.x, 0, rand.y);
                    blocked = Physics.Raycast(stateParams.Agent.transform.position, attackPosition - stateParams.Agent.transform.position, out RaycastHit hit, Vector3.Distance(stateParams.Agent.transform.position, attackPosition));
                    if (!hit.rigidbody)
                    {
                        stateParams.Agent.SetDestination(attackPosition);
                        newDestValid = true;
                    }
                }
                lastUpdateTime = Time.fixedTime;
                //AI will stay within radius of the enemy
            }

        }
        else if (stateParams.Health != null && !stateParams.IsMediumHealth)
        {
            fsm.Switch(Heal.Instance);
        }
        else
        {
            fsm.Switch(Idle.Instance);
        }
    }
}
public sealed class Chase : State
{

    public static readonly Chase Instance = new Chase();

    public override void Execute(FSM fsm, StateParams stateParams)
    {
        if (stateParams.IsMediumHealth && stateParams.IsTargetinRange)
        {
            //Target is close enough, stop and switch to attack mode
            fsm.Switch(Combat.Instance);

        }
        else if (stateParams.IsMediumHealth && stateParams.IsTargetClose)
        {
            //Target is too far
            //Intercept target
            GameObject nearestPlayer = stateParams.NearestPlayer;
            float targetDistance = Vector3.Distance(nearestPlayer.transform.position, stateParams.Agent.transform.position);
            float lookAheadTime = Mathf.Clamp(targetDistance / stateParams.Agent.speed, 0, 5);
            VelocityReporter reporter = nearestPlayer.GetComponent<VelocityReporter>();
            Vector3 futureTarget = nearestPlayer.transform.position + lookAheadTime * reporter.velocity;
            stateParams.Agent.SetDestination(futureTarget);
            // stateParams.Agent.SetDestination(stateParams.NearestPlayer.transform.position);
        }
        else
        {
            //No target selected
            fsm.Switch(Idle.Instance);
        }
    }
}
public sealed class CollectWeapon : State
{

    public static readonly CollectWeapon Instance = new CollectWeapon();

    public override void Execute(FSM fsm, StateParams stateParams)
    {

        if (!stateParams.Attributes.IsUnderAttack && stateParams.Weapon != null)
        {
            stateParams.Agent.SetDestination(stateParams.Weapon.transform.position);
        }
        else
        {
            fsm.Switch(Idle.Instance);
        }

        // Attack while collecting weapon
        if (stateParams.IsTargetinRange)
        {
            Utils.Attack(stateParams);
        }

    }
}
public sealed class Heal : State
{

    public static readonly Heal Instance = new Heal();

    public override void Execute(FSM fsm, StateParams stateParams)
    {
        if (!stateParams.IsGoodHealth && stateParams.Health != null)
        {
            stateParams.Agent.SetDestination(stateParams.Health.transform.position);
        }
        else
        {
            fsm.Switch(Idle.Instance);
        }

        // Attack while healing
        if (stateParams.IsTargetinRange)
        {
            Utils.Attack(stateParams);
        }
    }
}
abstract public class State
{
    abstract public void Execute(FSM fsm, StateParams stateParams);
}

public interface StateChangeListener
{
    void OnStateChange(State oldState, State newState);
}

public class FSM
{
    State state;

    private StateChangeListener listener;

    public void SetStateChangeListener(StateChangeListener stateChangeListener)
    {
        listener = stateChangeListener;
    }

    public void Switch(State newState)
    {
        listener?.OnStateChange(state, newState);
        state = newState;
        // Debug.Log("State: " + newState);
    }
    public void Execute(StateParams stateParams)
    {
        state.Execute(this, stateParams);
    }
}
