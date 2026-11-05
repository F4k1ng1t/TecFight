
using UnityEngine;

public class RunState : IFighterState
{
    private FighterStateMachine fsm;
    private int runDirection;
    private int idleFrames;

    private const float TURNAROUND_THRESHOLD = 0.8f;
    private const float IDLE_THRESHOLD = 0.2f;
    private const int IDLE_DELAY_FRAMES = 10;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        runDirection = fsm.direction;
        idleFrames = 0;

        fsm.animator?.PlayAnimation("Dash");
    }

    public void Exit()
    {
    }

    public void Update()
    {
        float moveX = fsm.input.MoveInput.x;

        if (Mathf.Abs(moveX) > TURNAROUND_THRESHOLD &&
            Mathf.Sign(moveX) != runDirection)
        {
            fsm.SetState(new RunTurnaroundState());
            return;
        }

        if (fsm.input.IsHoldingJump)
        {
            fsm.SetState(new JumpState());
            return;
        }

        if (Mathf.Abs(moveX) < IDLE_THRESHOLD)
        {
            idleFrames++;

            if (idleFrames >= IDLE_DELAY_FRAMES)
            {
                fsm.SetState(new IdleState());
                return;
            }
        }
        else
        {
            idleFrames = 0;
        }

        if (fsm.input.Special)
        {
            fsm.SetStateAttack(fsm.getAttack.GetSpecial());
            return;
        }

        if (fsm.input.Strong)
        {
            fsm.SetStateAttack(fsm.getAttack.GetStrong());
            return;
        }

        if (fsm.input.Attack)
        {
            fsm.SetStateAttack(fsm.getAttack.GetAttack());
            return;
        }
    }

    public void FixedUpdate()
    {
        if (fsm.physics == null)
            return;

        float moveX = runDirection * fsm.charObj.runSpeed;
        fsm.physics.SetHorizontalVelocity(moveX);
    }
}
