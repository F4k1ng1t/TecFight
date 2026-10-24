using UnityEngine;

public class KnockdownRollState : IFighterState
{
    const int startDodge = 4;
    const int endDodge = 12;
    const int moveEndFrame = 14;
    const int endState = 20;
    const float dodgeDistance = 2.2f;
    private int frame = 0;

    FighterStateMachine fsm;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator?.PlayAnimation("KnockdownRollGetup");
    }
    public void Exit()
    {
        fsm.isDodging = false;
    }

    public void FixedUpdate()
    {
        frame++;
        if (!fsm.animator.IsPlaying())
        {
            fsm.SetState(new IdleState());
        }
        if (frame < moveEndFrame)
        {
            fsm.transform.position += Vector3.right * (dodgeDistance / (moveEndFrame)) * fsm.direction;
        }
        if (frame < endDodge)
        {
            fsm.isDodging = false;
            return;
        }
        if (frame < startDodge)
        {
            fsm.isDodging = true;
        }
    }
    public void Update()
    {

    }
}
