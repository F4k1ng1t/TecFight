using UnityEngine;

public class SpotDodgeState : IFighterState
{
    const int startDodge = 3;
    const int endDodge = 14;
    const int endState = 20;
    private int frame = 0;
    FighterStateMachine fsm;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator?.PlayAnimation("SpotDodge");
    }
    public void Exit()
    {
        fsm.sheildHealth -= endState / 10; //60 * decay rate wich is 5 for now, make that globally facing later
        fsm.isDodging = false;
    }

    public void FixedUpdate()
    {
        frame++;
        if (startDodge == frame)
        {
            fsm.isDodging = true;
        }
        if (endDodge == frame)
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
