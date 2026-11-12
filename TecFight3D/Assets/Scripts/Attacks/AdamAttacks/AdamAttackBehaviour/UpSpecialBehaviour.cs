
using UnityEngine;

[CreateAssetMenu(fileName = "UpSpecialBehaviour", menuName = "Attack Behaviours/Up Special")]
public class UpSpecialBehaviour : AttackBehaviour
{
    private enum Phase
    {
        Charging,
        Traveling,
        SpecialFall
    }

    [Header("Charge")]
    [Min(0.01f)] public float maxChargeTime = 1f;

    [Header("Travel")]
    [Min(0f)] public float minTravelDistance = 3f;
    [Min(0f)] public float maxTravelDistance = 12f;
    [Min(0.01f)] public float travelSpeed = 20f;

    [Header("Special Fall")]
    [Min(0f)] public float fallSpeed = 12f;

    [Header("Rotation")]
    public float rotationSpeed = 720f;
    public float rotationOffset = 0f;

    [Header("Animations")]
    public string chargeAnimation;
    public string launchAnimation;
    public string specialFallAnimation;

    private Phase phase;
    private FighterStateMachine fsm;
    private FighterPhysics physics;

    private Transform characterVisual;
    private Quaternion originalVisualRotation;

    private float chargeTime;
    private float remainingDistance;

    private Vector2 aimDirection;
    private bool previousSpecialHeld;

    private bool originalGravityActive;

    public override bool CanExit => false;
    public override bool ControlsAirMovement => true;

    public override void OnAttackEnter(FighterStateMachine f, AttackObject attack)
    {
        fsm = f;
        physics = fsm.physics;

        phase = Phase.Charging;
        chargeTime = 0f;
        remainingDistance = 0f;
        aimDirection = Vector2.up;

        characterVisual = fsm.animator != null
            ? fsm.animator.transform
            : null;

        if (characterVisual != null)
            originalVisualRotation = characterVisual.localRotation;

        FighterInput input = fsm.input;
        previousSpecialHeld = input != null && input.Special;

        if (physics != null)
        {
            originalGravityActive = physics.useGravity;

            physics.useGravity = false;
            physics.ClearVelocity();
        }

        if (!string.IsNullOrEmpty(chargeAnimation))
            fsm.animator?.PlayAnimation(chargeAnimation);
    }

    public override void OnAttackUpdate(FighterStateMachine f, AttackObject attack)
    {
        if (phase != Phase.Charging)
            return;

        FighterInput input = fsm.input;

        if (input == null)
            return;

        Vector2 directionInput = input.MoveInput;

        if (directionInput.sqrMagnitude > 0.01f)
            aimDirection = directionInput.normalized;

        bool released = previousSpecialHeld && !input.Special;
        previousSpecialHeld = input.Special;

        if (released || chargeTime >= maxChargeTime)
            BeginTravel();
    }

    public override void OnAttackFixedUpdate(FighterStateMachine f, AttackObject attack)
    {
        if (physics == null)
            return;

        switch (phase)
        {
            case Phase.Charging:
                physics.useGravity = false;
                physics.SetVelocity(Vector3.zero);

                UpdateRotation();

                chargeTime = Mathf.Min(
                    chargeTime + Time.fixedDeltaTime,
                    maxChargeTime
                );
                break;

            case Phase.Traveling:
                physics.useGravity = false;

                UpdateRotation();
                UpdateTravel();
                break;

            case Phase.SpecialFall:
                physics.useGravity = true;
                UpdateSpecialFall();
                break;
        }
    }

    private void BeginTravel()
    {
        if (phase != Phase.Charging)
            return;

        phase = Phase.Traveling;

        float chargeRatio = Mathf.Clamp01(chargeTime / maxChargeTime);

        remainingDistance = Mathf.Lerp(
            minTravelDistance,
            maxTravelDistance,
            chargeRatio
        );

        physics.SetVelocity(
            new Vector3(
                aimDirection.x * travelSpeed,
                aimDirection.y * travelSpeed,
                0f
            )
        );

        if (!string.IsNullOrEmpty(launchAnimation))
            fsm.animator?.PlayAnimation(launchAnimation);
    }

    private void UpdateRotation()
    {
        if (characterVisual == null || aimDirection.sqrMagnitude < 0.001f)
            return;

        float targetAngle =
            Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg
            + rotationOffset;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        characterVisual.rotation = Quaternion.RotateTowards(
            characterVisual.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    private void UpdateTravel()
    {
        float distanceThisStep = travelSpeed * Time.fixedDeltaTime;

        remainingDistance -= distanceThisStep;

        if (fsm.ecb.HitWall || fsm.ecb.HitCeiling || remainingDistance <= 0f)
            BeginSpecialFall();
    }

    private void BeginSpecialFall()
    {
        if (phase == Phase.SpecialFall)
            return;

        phase = Phase.SpecialFall;

        physics.useGravity = true;

        physics.SetVelocity(Vector3.zero);

        physics.SetVerticalVelocity(-fallSpeed);

        if (!string.IsNullOrEmpty(specialFallAnimation))
            fsm.animator?.PlayAnimation(specialFallAnimation);
    }

    private void UpdateSpecialFall()
    {
        if (fsm.ecb != null && fsm.ecb.IsGrounded)
            fsm.SetState(new IdleState());
    }

    public override void OnAttackExit(FighterStateMachine f, AttackObject attack)
    {
        if (physics != null)
            physics.useGravity = originalGravityActive;

        if (characterVisual != null)
            characterVisual.localRotation = originalVisualRotation;
    }
}
