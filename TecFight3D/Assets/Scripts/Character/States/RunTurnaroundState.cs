
using UnityEngine;

public class RunTurnaroundState : IFighterState
{
    private FighterStateMachine fsm;
    private FighterECB ecb;

    private int frames;

    private const int TURNAROUND_LENGTH = 10;
    private const float TURNAROUND_SPEED = 3f;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        ecb = f.GetComponent<FighterECB>();

        frames = 0;

        fsm.FlipDirection();
        fsm.animator?.PlayAnimation("DashTurnaround");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (fsm.input.IsHoldingJump)
        {
            fsm.SetState(new JumpState());
        }
    }

    public void FixedUpdate()
    {
        if (ecb == null) return;

        Vector3 displacement = new Vector3(
            fsm.direction * TURNAROUND_SPEED * Time.fixedDeltaTime,
            0f,
            0f
        );

        ecb.Move(displacement);

        frames++;

        if (frames >= TURNAROUND_LENGTH)
        {
            fsm.SetState(new RunState());
        }
    }
}
