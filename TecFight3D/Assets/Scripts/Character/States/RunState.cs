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
        fsm.animator.PlayAnimation("Dash", true, 0);
    }
    public void Exit()
    {

    }
    public void Update()
    {
        float directionalInput = fsm.input.MoveInput.x * initDirection;
        if (directionalInput < -0.8f)
        {
            fsm.SetState(new RunTurnaroundState());
            return;
        }
        if (fsm.input.IsHoldingJump)
        {
            fsm.SetState(new JumpState());
            return;
        }
        if (Mathf.Abs(fsm.input.MoveInput.x) < 0.2f)
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
