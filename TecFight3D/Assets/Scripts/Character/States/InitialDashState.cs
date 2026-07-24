using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InitialDashState : IFighterState
{
    FighterStateMachine fsm;
    int frames = 0;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        frames = 0;
    }
    public void Exit()
    {

    }
    public void Update()
    {

        

    }
    public void FixedUpdate()
    {
        fsm.rig.linearVelocity = new Vector3(fsm.direction * 10f, fsm.rig.linearVelocity.y, 0);
        frames++;
        if (frames == 10)
        {
            if (fsm.input.MoveInput.x != 0)
            {
                fsm.SetState(new RunState());
            }
            else
            {
                fsm.rig.linearVelocity = new Vector3(0, fsm.rig.linearVelocity.y, 0);
                fsm.SetState(new IdleState());
            }
        }
        if (fsm.input.Smash && fsm.direction != fsm.input.SmashDirection)
        {
            fsm.FlipDirection();
            fsm.input.ConsumeSmash();
            fsm.SetState(new InitialDashState());
        }
    }
}
