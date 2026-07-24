using NUnit;
using UnityEngine;

public class RunState : IFighterState
{
    FighterStateMachine fsm;
    int frames = 0;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
    }
    public void Exit()
    {

    }
    public void Update()
    {
        
    }
    public void FixedUpdate()
    {
        fsm.rig.linearVelocity = new Vector3(fsm.direction * 9f, fsm.rig.linearVelocity.y, 0);


        if (fsm.direction != fsm.input.SmashDirection)
        {
            fsm.SetState(new RunTurnaroundState());
        }
        if (fsm.input.JumpPressed)
        {
            fsm.SetState(new JumpState());
        }
        if (fsm.input.MoveInput.x < 0.2f && fsm.input.MoveInput.x > -0.2f)
        {
            frames++;
            if (frames == 10)
            {
                fsm.rig.linearVelocity = new Vector3(0, fsm.rig.linearVelocity.y, 0);
                fsm.SetState(new IdleState());
            }
        }
    }
}
