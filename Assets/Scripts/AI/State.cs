using UnityEngine;
using UnityEngine.AI;


//State classes
public sealed class Idle : State
{

    public static readonly Idle Instance = new Idle();

    bool isWayPointvalid = false;

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

            if (stateParams.Agent.remainingDistance < 0.5)
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
    public override void Execute(FSM fsm, StateParams stateParams)
    {
        if (stateParams.Target != null && stateParams.IsTargetClose && stateParams.Attributes.equippedWeapons[stateParams.Attributes.activeWeaponIndex].GetComponent<Weapon>().Ammo > 0)
        {
            Vector3 targetDirection = stateParams.Target.transform.position - stateParams.Agent.transform.position;
            stateParams.Agent.transform.rotation = Quaternion.Lerp(stateParams.Agent.transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 10f);
            stateParams.Agent.updateRotation = true;
            stateParams.WeaponController.Attack(stateParams.Target.transform, WeaponFireType.SINGLE);

            if (stateParams.Agent.remainingDistance - stateParams.Agent.stoppingDistance < 0.5f || Time.fixedTime - lastUpdateTime > PositionUpdateFrequency)
            {
                Vector3 attackPosition = stateParams.Target.transform.position;
                Vector2 rand = Random.insideUnitCircle * AttackRadius;
                attackPosition += new Vector3(rand.x, 0, rand.y);
                stateParams.Agent.SetDestination(attackPosition);
                lastUpdateTime = Time.fixedTime;
                //AI will stay within radius of the enemy
            }

            // stateParams.Agent.transform.position += 2*stateParams.Target.transform.forward * stateParams.Agent.speed * Time.deltaTime;

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
