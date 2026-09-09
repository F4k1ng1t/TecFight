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
        if(fsm.input.Flick)
        {
            float flickX = fsm.input.FlickDirection.x;
            if (Mathf.Abs(flickX) > 0.8f)
            {
                fsm.SetDirection(flickX > 0);
                fsm.input.ConsumeFlick();
                fsm.SetState(new InitialDashState());

                return;
            }
        }
        //if (Mathf.Abs(fsm.input.MoveInput.x) > 0.2f)
        //{
        //    fsm.input.ConsumeFlick();
        //    fsm.SetDirection(fsm.input.MoveInput.x > 0);
        //    fsm.SetState(new WalkState());

        //    return;
        //}
        if (fsm.input.JumpPressed)
        {
            fsm.SetState(new JumpState());
        }
    }
    public void FixedUpdate()
    {

    }
}
