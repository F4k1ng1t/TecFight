
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
    private Transform characterVisual;
    private Quaternion originalVisualRotation;
    private float chargeTime;
    private float remainingDistance;
    private Vector2 aimDirection;
    private bool previousSpecialHeld;

    public override bool CanExit => false;
    public override bool ControlsAirMovement => true;

    public override void OnAttackEnter(FighterStateMachine f, AttackObject attack)
    {
        fsm = f;
        phase = Phase.Charging;
        chargeTime = 0f;
        remainingDistance = 0f;
        aimDirection = Vector2.up;

        characterVisual = fsm.animator != null
            ? fsm.animator.transform
            : null;

        if (characterVisual != null)
            originalVisualRotation = characterVisual.localRotation;

        FighterInput input = fsm.GetComponent<FighterInput>();
        previousSpecialHeld = input != null && input.Special;

        fsm.gravity.active = false;

        if (!string.IsNullOrEmpty(chargeAnimation))
            fsm.animator.PlayAnimation(chargeAnimation);
    }

    public override void OnAttackUpdate(FighterStateMachine f, AttackObject attack)
    {
        if (phase != Phase.Charging)
            return;

        FighterInput input = fsm.GetComponent<FighterInput>();

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
        switch (phase)
        {
            case Phase.Charging:
                fsm.gravity.active = false;
                UpdateRotation();
                chargeTime = Mathf.Min(chargeTime + Time.fixedDeltaTime, maxChargeTime);
                
                break;

            case Phase.Traveling:
                fsm.gravity.active = false;
                UpdateRotation();
                UpdateTravel();
                break;

            case Phase.SpecialFall:
                fsm.gravity.active = true;
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

        if (!string.IsNullOrEmpty(launchAnimation))
            fsm.animator.PlayAnimation(launchAnimation);
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
        if (remainingDistance <= 0f)
        {
            BeginSpecialFall();
            return;
        }

        float distanceThisStep = Mathf.Min(
            travelSpeed * Time.fixedDeltaTime,
            remainingDistance
        );

        Vector3 displacement = new Vector3(
            aimDirection.x,
            aimDirection.y,
            0f
        ) * distanceThisStep;

        Vector3 resolvedMovement = fsm.ecb.Move(displacement);

        remainingDistance -= resolvedMovement.magnitude;

        if (fsm.ecb.HitWall || fsm.ecb.HitCeiling)
        {
            BeginSpecialFall();
            return;
        }

        if (remainingDistance <= 0.001f)
            BeginSpecialFall();
    }

    private void BeginSpecialFall()
    {
        if (phase == Phase.SpecialFall)
            return;

        phase = Phase.SpecialFall;

        if (!string.IsNullOrEmpty(specialFallAnimation))
            fsm.animator.PlayAnimation(specialFallAnimation);
    }

    private void UpdateSpecialFall()
    {
        fsm.ecb.Move(Vector3.down * fallSpeed * Time.fixedDeltaTime);

        if (fsm.ecb.IsGrounded)
            fsm.SetState(new IdleState());
    }

    public override void OnAttackExit(FighterStateMachine f, AttackObject attack)
    {
        fsm.gravity.active = true;

        if (characterVisual != null)
            characterVisual.localRotation = originalVisualRotation;
    }
}
