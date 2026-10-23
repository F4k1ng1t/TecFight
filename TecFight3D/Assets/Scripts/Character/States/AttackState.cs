using UnityEngine;

public class AttackState : IFighterState
{
    FighterStateMachine fsm;
    AttackObject ao;
    public void Enter(FighterStateMachine f, AttackObject a)
    {
        fsm = f;
        ao = a;
        fsm.animator.PlayAnimation(a.animationName);
        //Debug.Log("Attack");
    }
    public void Exit()
    {

    }
    public void Update()
    {
        if (fsm.animator.IsPlaying())
        {
            return;
        }
        if (ao.aerial)
        {
            Debug.Log("Attack 2 Air");
            fsm.SetState(new AirState());
        }
        else
        {
            Debug.Log("Attack 2 Idle");
            fsm.SetState(new IdleState());
        }
        //fsm.SetState(new IdleState());
    }
    public void FixedUpdate()
    {
        if (ao.aerial)
        {
            fsm.ApplyAirMovement();
        }
    }
}
