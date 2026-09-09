using NUnit;
using UnityEngine;

public class RunState : IFighterState
{
    FighterStateMachine fsm;
    int initDirection;
    int frames = 0;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        initDirection = fsm.direction;
    }
    public void Exit()
    {

    }
    public void Update()
    {
        


        if (fsm.input.Flick)
        {
            fsm.input.ConsumeFlick();
            float flickX = fsm.input.FlickDirection.x;

            // Horizontal flick opposite our current direction.
            if (Mathf.Abs(fsm.input.MoveInput.x) > 0.8 && Mathf.Sign(fsm.input.MoveInput.x) != Mathf.Sign(initDirection))
            {

                fsm.SetState(new RunTurnaroundState());
                return;
            }
        }
        if (fsm.input.JumpPressed)
        {
            fsm.SetState(new JumpState());
            return;
        }
        if (fsm.input.MoveInput.x < 0.2f && fsm.input.MoveInput.x > -0.2f)
        {
            frames++;
            if (frames == 10)
            {
                fsm.rig.linearVelocity = new Vector3(0, fsm.rig.linearVelocity.y, 0);
                fsm.SetState(new IdleState());
                return;
            }
        }
    }
    public void FixedUpdate()
    {
        fsm.rig.linearVelocity = new Vector3(fsm.direction * 9f, fsm.rig.linearVelocity.y, 0);
    }
}
