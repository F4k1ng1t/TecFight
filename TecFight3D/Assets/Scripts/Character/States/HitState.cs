using UnityEngine;

public class HitState : IFighterState
{
    FighterStateMachine fsm;
    Vector3 kb;
    int hitstunLength;
    int hitstunFrames = 0;

    public bool AllowGroundedTransitions => false;
    public HitState(Vector3 kbv)
    {
        kb = kbv;
    }
    public void Enter(FighterStateMachine f)
    {
        //this handles the animations and hitstun
        // another script does the knockback
        
        
        fsm = f;

        if (fsm.ecb.IsGrounded && kb.y < 0)
        {
            kb.Set(kb.x, Mathf.CeilToInt(-kb.y / 2), 0f);
        }
        hitstunLength = 3 * (int)(kb.magnitude);
        hitstunLength = hitstunLength < 10 ? 10 : hitstunLength;
        
        fsm.physics.ClearVelocity();
        fsm.physics.AddForce(kb, ForceMode.VelocityChange);
        
        fsm.SetDirection(-kb.x);
        if (Mathf.Abs(kb.x) >= Mathf.Abs(kb.y))
        {
            fsm.animator.PlayAnimation("HitStunSide");
        }
        else
        {
            fsm.animator.PlayAnimation("HitStunUp");
        }
        Debug.Log($"Hitstun Length : {hitstunLength} frames");
        
    }

    public void Exit()
    {

    }

    public void FixedUpdate()
    {
        hitstunFrames++;
        if (fsm.ecb.IsGrounded)
        {
            if (hitstunFrames > hitstunLength / 2)
            {
                fsm.SetState(new KnockdownState());
                return;
            }
            else if (kb.y < 0 && hitstunFrames <= hitstunLength / 2)
            {
                fsm.SetState(new HitState(new Vector3(kb.x, -kb.y / 2, 0f)));
                return;
            }
        }
        else if (!fsm.ecb.IsGrounded && hitstunFrames >= hitstunLength)
        {
            fsm.SetState(new AirState());
            return;
        }
    }

    public void Update()
    {

    }
}
