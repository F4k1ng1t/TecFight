using UnityEngine;

public class WavelandState : IFighterState
{
    private FighterStateMachine fsm;

    private const int WAVELAND_LENGTH = 10;
    int WavelandFrames = 0;
    public bool AllowGroundedTransitions => false;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.currentDJC = fsm.charObj.doubleJumpCount;
        fsm.hasAirdodge = true;
        fsm.animator?.PlayAnimation("DodgeLand");
    }

    public void Update()
    {
        //if (fsm.animator != null && fsm.animator.IsAnimationFinished("DodgeLand"))
        //    fsm.SetState(new IdleState());
    }

    public void FixedUpdate()
    {
        WavelandFrames++;
        if (WavelandFrames >= WAVELAND_LENGTH)
        {
            fsm.SetState(new IdleState());
            return;
        }
        if (!fsm.ecb.IsGrounded)
        {
            fsm.SetState(new AirState());
            return;
        }
        fsm.GroundedActionable();
    }

    public void Exit()
    {
    }
}