
using UnityEngine;

public class InitialDashState : IFighterState
{
    private FighterStateMachine fsm;
    private FighterECB ecb;

    private int frames;
    private int dashDirection;

    private const int DASH_LENGTH = 20;
    private const float INPUT_THRESHOLD = 0.2f;
    private const float FLICK_THRESHOLD = 0.5f;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        ecb = f.GetComponent<FighterECB>();

        frames = 0;
        dashDirection = f.direction;

        // Direction of the dash when we entered.
        dashDirection = fsm.direction;
        fsm.animator.PlayAnimation("Dash");
    }

    public void Exit() { }

    public void Update()
    {
        if (fsm.input.IsHoldingJump)
        {
            fsm.SetState(new JumpState());
            return;
        }

        if (fsm.input.Flick)
        {
            float flickX = fsm.input.FlickDirection.x;

            if (Mathf.Abs(flickX) > FLICK_THRESHOLD &&
                Mathf.Sign(flickX) == -dashDirection)
            {
                fsm.input.ConsumeFlick();
                fsm.FlipDirection();
                fsm.SetState(new InitialDashState());
                return;
            }
        }

        if (fsm.input.Attack)
        {
            fsm.SetStateAttack(fsm.getAttack.GetAttack());
            return;
        }

        if (fsm.input.Shield)
        {
            fsm.SetState(new ShieldState());
        }
    }

    public void FixedUpdate()
    {
        if (ecb == null) return;

        float moveX = dashDirection * fsm.charObj.initialDashSpeed;

        // Preserve dash momentum for jumping and air drift.
        fsm.gravity.horizontalVelocity = moveX;

        Vector3 displacement = new Vector3(
            moveX * Time.fixedDeltaTime,
            0f,
            0f
        );

        ecb.Move(displacement);

        frames++;

        if (frames >= DASH_LENGTH)
        {
            float inputX = fsm.input.MoveInput.x;

            if (Mathf.Abs(inputX) > INPUT_THRESHOLD)
            {
                if (inputX * dashDirection < -INPUT_THRESHOLD)
                {
                    fsm.SetState(new RunTurnaroundState());
                }
                else
                {
                    fsm.SetState(new RunState());
                }
            }
            else
            {
                fsm.SetState(new IdleState());
            }
        }
    }
}
