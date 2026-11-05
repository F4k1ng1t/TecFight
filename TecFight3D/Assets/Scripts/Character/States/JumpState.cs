
using UnityEngine;

public class JumpState : IFighterState
{
    private FighterStateMachine fsm;
    private CharacterObject co;
    private FighterInputProcesser fip;

    private int frames;
    private const int JUMPSQUAT_LENGTH = 4;
    private bool jumpExecuted;

    private float savedHorizontalVelocity;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = f.charObj;
        fip = f.fip;

        frames = 0;
        jumpExecuted = false;

        savedHorizontalVelocity = fsm.physics.Velocity.x;

        fsm.animator?.PlayAnimation("JumpSquat");
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }

    public void FullHop()
    {
        fsm.physics.SetVelocity(new Vector3(
            Mathf.Clamp(savedHorizontalVelocity, -co.airSpeed, co.airSpeed),
            co.fullhopForce,
            0f
        ));

        fsm.animator?.PlayAnimation("Hop");
    }

    public void ShortHop()
    {
        fsm.physics.SetVelocity(new Vector3(
            savedHorizontalVelocity,
            co.shorthopForce,
            0f
        ));

        fsm.animator?.PlayAnimation("Hop");
    }

    public void FixedUpdate()
    {
        if (jumpExecuted)
            return;

        frames++;

        if (frames >= JUMPSQUAT_LENGTH)
        {
            jumpExecuted = true;

            if (fip.IsHoldingJump())
                FullHop();
            else
                ShortHop();

            fsm.SetState(new AirState());
        }
    }
}
