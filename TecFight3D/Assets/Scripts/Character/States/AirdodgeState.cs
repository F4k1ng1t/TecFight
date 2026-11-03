
using UnityEngine;

public class AirdodgeState : IFighterState
{
    private const float DODGE_DURATION = 0.25f;
    private const float DODGE_SPEED = 18f;
    private const float RECOVERY_DURATION = 0.45f;

    private FighterStateMachine fsm;
    private Gravity gravity;

    private Vector3 dodgeDirection;
    private float dodgeTimer;
    private float recoveryTimer;

    private bool dodging;
    private bool originalGravityActive;
    private bool originalClearHorizontalVelocity;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        gravity = fsm.gravity;
        fsm.hasAirdodge = false;

        dodgeTimer = 0f;
        recoveryTimer = 0f;
        dodging = true;

        Vector2 input = fsm.input.MoveInput;

        if (input.sqrMagnitude < 0.01f)
            input = new Vector2(fsm.direction, 0f);

        dodgeDirection = new Vector3(input.x, input.y, 0f).normalized;

        if (gravity != null)
        {
            originalGravityActive = gravity.active;
            originalClearHorizontalVelocity = gravity.clearHorizontalVelocityOnLanding;

            gravity.active = false;
            gravity.verticalVelocity = 0f;
            gravity.horizontalVelocity = 0f;
            gravity.clearHorizontalVelocityOnLanding = false;
        }

        fsm.animator?.PlayAnimation("Airdodge");
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
        if (!dodging || fsm.ecb == null)
            return;

        Vector3 displacement = dodgeDirection * DODGE_SPEED * Time.fixedDeltaTime;
        Vector3 resolved = fsm.ecb.Move(displacement);

        if ((displacement - resolved).sqrMagnitude > 0.0001f)
            EndDodge();
    }

    private void EndDodge()
    {
        if (!dodging)
            return;

        dodging = false;
        recoveryTimer = 0f;

        if (gravity != null)
        {
            gravity.horizontalVelocity = 0f;
            gravity.verticalVelocity = 0f;
            gravity.active = originalGravityActive;
        }
    }

    private void FinishAirdodge(bool grounded)
    {
        if (gravity != null)
        {
            gravity.horizontalVelocity = 0f;
            gravity.verticalVelocity = 0f;
            gravity.active = originalGravityActive;
            gravity.clearHorizontalVelocityOnLanding = originalClearHorizontalVelocity;
        }

        if (grounded)
            fsm.SetState(new IdleState());
        else
            fsm.SetState(new AirState());
    }

    public void Exit()
    {
        if (gravity != null)
        {
            gravity.horizontalVelocity = 0f;
            gravity.verticalVelocity = 0f;
            gravity.active = originalGravityActive;
            gravity.clearHorizontalVelocityOnLanding = originalClearHorizontalVelocity;
        }
    }
}
