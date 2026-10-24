using UnityEngine;

public class HitState : IFighterState
{
    FighterStateMachine fsm;
    public void Enter(FighterStateMachine f)
    {
        //this handles the animations and hitstun
        // another script does the knockback

        fsm = f;
        fsm.animator.PlayAnimation("HitStunSide");
    }

    public void Exit()
    {

    }

    public void FixedUpdate()
    {
        if (fsm.ecb.IsGrounded)
        {
            fsm.SetState(new KnockdownState());
        }
    }

    void IFighterState.Update()
    {

    }
}
