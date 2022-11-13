using UnityEngine;
using UnityEngine.AI;


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
        // Debug.Log("Switch to Attack? = " + (stateParams.Target != null && stateParams.IsTargetinRange && stateParams.IsGoodHealth) + "; Target: " + stateParams.Target
        //         + " IsTargetinRange: " + stateParams.IsTargetinRange + "GoodHealth: " + stateParams.IsGoodHealth);

        // Debug.Log("Switch to Chase? = " + (stateParams.Target != null && stateParams.IsGoodHealth && stateParams.IsTargetClose) + " Target: " + stateParams.Target
        //         + " IsTargetClose: " + stateParams.IsTargetClose + " GoodHealth: " + stateParams.IsGoodHealth);

        if ((stateParams.Target != null && stateParams.IsTargetinRange && (stateParams.IsGoodHealth ||
            stateParams.Health == null)))
        {
            fsm.Switch(Attack.Instance);
        }
        else if (stateParams.Target != null && (stateParams.IsGoodHealth || stateParams.Health == null) && stateParams.IsTargetClose)
        {
            fsm.Switch(Chase.Instance);
        }

        else if (stateParams.Target != null && (stateParams.IsGoodHealth || stateParams.Health == null) && stateParams.Attributes.IsUnderAttack)
        {
            fsm.Switch(Chase.Instance);
        }

        else
        {
            //Select a nearby accessible waypoint to wander towards
            
            while (!isWayPointvalid && stateParams.Agent.remainingDistance < 0.5)
            {
                var offset = Random.insideUnitCircle * 20;
                var newWaypoint = stateParams.Agent.transform.position + new Vector3(offset.x, 0.0f, offset.y);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(newWaypoint, out hit, 1f, NavMesh.AllAreas))
                {
                    stateParams.Agent.SetDestination(hit.position);
                    stateParams.Waypoint = stateParams.Agent.transform.position + new Vector3(offset.x, 0.0f, offset.y);
                    isWayPointvalid = true;
                    // Debug.Log(hit.position);
                }

            }

            if (stateParams.Agent.remainingDistance < 0.5 || stateParams.Agent.isStopped)
            {
                isWayPointvalid = false;
            }




        }
    }
}

public sealed class Attack : State
{
    public static readonly Attack Instance = new Attack();

    public float PositionUpdateFrequency = 3f;
    public float AttackRadius = 10f;
    private float lastUpdateTime = 0f;
    private bool blocked = true;
    private NavMeshHit hit;
    public override void Execute(FSM fsm, StateParams stateParams)
    {

        
        if (stateParams.Target != null && stateParams.IsTargetClose && stateParams.Attributes.equippedWeapons[stateParams.Attributes.activeWeaponIndex].GetComponent<Weapon>().Ammo > 0)
        {

            Vector3 targetDirection = stateParams.Target.transform.position - stateParams.Agent.transform.position;
            stateParams.Agent.transform.rotation = Quaternion.Lerp(stateParams.Agent.transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 45f);
            stateParams.Agent.updateRotation = true;

            blocked = Physics.Raycast(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f);
            Debug.DrawRay(stateParams.Agent.transform.position, stateParams.Agent.transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);

            if (hitInfo.transform.CompareTag("Player"))
                {
                stateParams.WeaponController.Attack(stateParams.Target.transform, WeaponFireType.SINGLE);
                }

            if (stateParams.Agent.remainingDistance < 0.5f || Time.fixedTime - lastUpdateTime > PositionUpdateFrequency)
                {
                    Debug.Log("Time check: " + (Time.fixedTime - lastUpdateTime));
                    bool newDestValid = false;
                    Vector3 attackPosition = stateParams.Target.transform.position;
                    while (newDestValid == false) {
                        Vector2 rand = Random.insideUnitCircle * AttackRadius;
                        attackPosition += new Vector3(rand.x, 0, rand.y);
                        blocked = Physics.Raycast(stateParams.Agent.transform.position, attackPosition - stateParams.Agent.transform.position, out RaycastHit hit, Vector3.Distance(stateParams.Agent.transform.position, attackPosition));
                        if (!hit.rigidbody) { 
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

        else if (stateParams.Agent.velocity.magnitude < 0.15f) //workaround to get Agent unstuck if he is is "stuck" at a point
        {
            Debug.Log("AIEnemy velocity: " + stateParams.Agent.velocity.magnitude);
            fsm.Switch(Idle.Instance);
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
        if (stateParams.Target != null && stateParams.IsTargetinRange)
        {
            //Target is close enough, stop and switch to attack mode
            stateParams.Agent.SetDestination(stateParams.Agent.transform.position);
            fsm.Switch(Attack.Instance);
        }
        else if (stateParams.Target != null)
        {
            //Target is too far
            stateParams.Agent.SetDestination(stateParams.Target.transform.position);
        }

        else if (stateParams.Health != null && !stateParams.IsMediumHealth)
        {
            fsm.Switch(Heal.Instance);
        }

        else
        {
            //No target selected
            fsm.Switch(Idle.Instance);
        }
    }
}
public sealed class Heal : State
{

    public static readonly Heal Instance = new Heal();

    public override void Execute(FSM fsm, StateParams stateParams)
    {
        if (stateParams.Target != null && stateParams.IsTargetinRange && (stateParams.IsGoodHealth || stateParams.IsMediumHealth))
        {
            fsm.Switch(Attack.Instance);
        }
        else if (stateParams.Target != null && stateParams.IsGoodHealth && stateParams.IsTargetClose)
        {
            fsm.Switch(Chase.Instance);
        }

        else if (stateParams.Target != null && (stateParams.IsGoodHealth || stateParams.IsMediumHealth) && stateParams.Attributes.IsUnderAttack)
        {
            fsm.Switch(Chase.Instance);
        }

        else if (((stateParams.IsGoodHealth || stateParams.IsMediumHealth) && !stateParams.Attributes.IsUnderAttack && !stateParams.IsTargetClose) || stateParams.Target == null)
        {
            fsm.Switch(Idle.Instance);
        }

        else if (stateParams.Health != null && !stateParams.IsMediumHealth)
        {
            //Vector3 healthDirection = stateParams.Health.transform.position - stateParams.Agent.transform.position;
            //stateParams.Agent.transform.rotation = Quaternion.Lerp(stateParams.Agent.transform.rotation, Quaternion.LookRotation(healthDirection), Time.deltaTime * 10f);
            //stateParams.Agent.updateRotation = true;
            stateParams.Agent.SetDestination(stateParams.Health.transform.position);
        }

        else if (stateParams.Health == null)
        {
            if (stateParams.IsTargetinRange || stateParams.IsUnderAttack)
            {
                fsm.Switch(Attack.Instance);
            }
            else if (stateParams.IsTargetClose || stateParams.IsUnderAttack)
            {
                fsm.Switch(Chase.Instance);
            }
            else if ((!stateParams.IsTargetClose && !stateParams.IsUnderAttack) || stateParams.Target == null)
            {
                fsm.Switch(Idle.Instance);
            }
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
