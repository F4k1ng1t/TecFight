using UnityEngine;

public class HitState : IFighterState
{
    FighterStateMachine fsm;
    Vector3 kb;

    public HitState(Vector3 kbv)
    {
        kb = kbv;
    }
    public void Enter(FighterStateMachine f)
    {
        //this handles the animations and hitstun
        // another script does the knockback
        
        
        fsm = f;

        fsm.physics.ClearVelocity();
        fsm.physics.AddForce(kb, ForceMode.VelocityChange);
        fsm.SetDirection(-kb.x);
        if (kb.x >= kb.y)
        {
            fsm.animator.PlayAnimation("HitStunSide");
        }
        else
        {
            fsm.animator.PlayAnimation("HitStunUp");
        }
        
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

    public void Update()
    {

    }
}
