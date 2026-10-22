using UnityEngine;

public class DodgeRollState : IFighterState
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
        fsm.animator?.PlayAnimation("DodgeRoll", false, fsm.direction);
        fsm.isDodging = true;
    }
    public void Exit()
    {
        fsm.sheildHealth -= endState / 10; //60 * decay rate wich is 5 for now, make that globally facing later
        fsm.isDodging = false;
    }

    public void FixedUpdate()
    {
        frame++;
        if (frame < moveEndFrame)
        {
            fsm.transform.position += Vector3.right * (dodgeDistance / (moveEndFrame)) * fsm.direction;
        }
        if (frame == startDodge)
        {
            fsm.isDodging = true;
        }
        if (frame == endDodge)
        {
            fsm.isDodging = false;
        }
    }

    public void Update()
    {
        if(frame < endState)
        {
            return;
        }
        if (fsm.input.Shield)
        {
            fsm.SetState(new ShieldState());
            return;
        }
        fsm.SetState(new IdleState());
    }
}
