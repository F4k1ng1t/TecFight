
using UnityEngine;

public class AirdodgeState : IFighterState
{
    private const float DODGE_DURATION = 0.1f;
    private const float DODGE_SPEED = 12f;
    private const float RECOVERY_DURATION = 0.45f;

    private FighterStateMachine fsm;
    private FighterPhysics physics;

    private Vector3 dodgeDirection;
    private float dodgeTimer;
    private float recoveryTimer;

    private bool dodging;
    private bool originalGravityActive;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        physics = fsm.GetComponent<FighterPhysics>();
        fsm.hasAirdodge = false;

        dodgeTimer = 0f;
        recoveryTimer = 0f;
        dodging = true;

        Vector2 input = fsm.input.MoveInput;

        if (input.sqrMagnitude < 0.01f)
            input = new Vector2(fsm.direction, 0f);

        dodgeDirection = new Vector3(input.x, input.y, 0f).normalized;

        if (physics != null)
        {
            originalGravityActive = physics.useGravity;

            physics.useGravity = false;
            physics.SetVelocity(dodgeDirection * DODGE_SPEED);
        }

        fsm.animator?.PlayAnimation("AirDodge");
    }

    public void Update()
    {
        if (dodging)
        {
            dodgeTimer += Time.deltaTime;

            if (dodgeTimer >= DODGE_DURATION)
                EndDodge();

            return;
        }

        recoveryTimer += Time.deltaTime;

        if (fsm.ecb != null && fsm.ecb.IsGrounded)
        {
            FinishAirdodge(true);
            return;
        }

        if (recoveryTimer >= RECOVERY_DURATION)
            FinishAirdodge(false);
    }

    public void FixedUpdate()
    {
        if (!dodging || physics == null || fsm.ecb == null)
            return;

        // FighterPhysics normally handles movement.
        // Do not move the ECB directly here.
    }

    private void EndDodge()
    {
        if (!dodging)
            return;

        dodging = false;
        recoveryTimer = 0f;

        if (physics != null)
            physics.useGravity = originalGravityActive;
    }

    private void FinishAirdodge(bool grounded)
    {
        if (physics != null)
            physics.useGravity = originalGravityActive;

        if (grounded)
            fsm.SetState(new WavelandState());
        else
            fsm.SetState(new AirState());
    }

    public void Exit()
    {
        if (physics != null)
            physics.useGravity = originalGravityActive;
    }
}
