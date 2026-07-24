using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class IdleState : IFighterState
{
    FighterStateMachine fsm;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
    }
    public void Exit()
    {

    }
    public void Update()
    {
        if (fsm.input.MoveInput.x != 0)
        {
            fsm.SetDirection(fsm.input.MoveInput.x > 0);
            if (fsm.input.Smash)
            {
                fsm.SetState(new InitialDashState());
            }
            else
            {
                fsm.SetState(new WalkState());
            }
        }
        if(fsm.input.JumpPressed)
        {
            fsm.SetState(new JumpState());
        }
    }
    public void FixedUpdate()
    {

    }
}
