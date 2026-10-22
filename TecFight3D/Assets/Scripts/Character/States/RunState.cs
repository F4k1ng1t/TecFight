
using UnityEngine;

public class RunState : IFighterState
{
    private FighterStateMachine fsm;
    private FighterECB ecb;
    private int runDirection;
    private int idleFrames;

    private const float TURNAROUND_THRESHOLD = 0.8f;
    private const float IDLE_THRESHOLD = 0.2f;
    private const int IDLE_DELAY_FRAMES = 10;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        ecb = f.GetComponent<FighterECB>();

        runDirection = f.direction;
        idleFrames = 0;

        fsm.animator?.PlayAnimation("Dash", true, 0);
    }

    public void Exit() { }

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
    }

    public void FixedUpdate()
    {
        if (ecb == null) return;

        float moveX = runDirection * fsm.charObj.runSpeed;

        // Store horizontal momentum for jumping and air drift.
        fsm.gravity.horizontalVelocity = moveX;

        ecb.Move(new Vector3(
            moveX * Time.fixedDeltaTime,
            0f,
            0f
        ));
    }
}
