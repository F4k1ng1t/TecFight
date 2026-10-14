using UnityEngine;

public class AttackState : IFighterState
{
    FighterStateMachine fsm;
    AttackObject ao;
    public void Enter(FighterStateMachine f, AttackObject a)
    {
        fsm = f;
        ao = a;
        fsm.animator.PlayAnimation(a.animationName, false, 0);
        Debug.Log("Attack");
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
        //something about input buffers triggering the second jab
        fsm.SetState(new IdleState());
    }
    public void FixedUpdate()
    {
        if (ao.aerial)
        {
            fsm.ApplyAirMovement();
        }
    }
}
