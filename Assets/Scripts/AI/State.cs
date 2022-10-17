using UnityEngine;

//State classes
public sealed class Idle : State
{

    public static readonly Idle Instance = new Idle();

    public override void Execute(FSM fsm, StateParams stateParams)
    {
        if (stateParams.Target != null && stateParams.IsTargetClose && stateParams.IsGoodHealth)
        {
            fsm.Switch(Attack.Instance);
        }
        else if (stateParams.Target != null && stateParams.IsGoodHealth)
        {
            fsm.Switch(Chase.Instance);
        }
        else
        {
            //Collect health or do some wandering
        }
    }
}

public sealed class Attack : State
{

    public static readonly Attack Instance = new Attack();

    public override void Execute(FSM fsm, StateParams stateParams)
    {
        if (stateParams.Target != null &&  stateParams.IsTargetClose && stateParams.Attributes.equippedWeapons[stateParams.Attributes.activeWeaponIndex].GetComponent<Weapon>().Ammo > 0)
        {
            Vector3 targetDirection = stateParams.Target.transform.position - stateParams.Agent.transform.position;
            stateParams.Agent.transform.rotation = Quaternion.Lerp(stateParams.Agent.transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 10f);
            stateParams.Agent.updateRotation = true;
            stateParams.WeaponController.Attack(stateParams.Target.transform, WeaponFireType.SINGLE);
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
        if (stateParams.Target != null && stateParams.IsTargetClose)
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
        else
        {
            //No target selected
            fsm.Switch(Idle.Instance);
        }
    }
}
abstract public class State
{
    abstract public void Execute(FSM fsm, StateParams stateParams);
}
public class FSM
{
    State state;
    public void Switch(State newState)
    {
        //TODO: Add Text in HUD above player (for debug mode)
        state = newState;
        Debug.Log("State: " + newState);
    }
    public void Execute(StateParams stateParams)
    {
        state.Execute(this, stateParams);
    }
}
