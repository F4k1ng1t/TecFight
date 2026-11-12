
using UnityEngine;

[CreateAssetMenu(fileName = "SideSpecialBehaviour", menuName = "Attack Behaviours/Side Special")]
public class SideSpecialBehaviour : AttackBehaviour
{
    private enum Phase
    {
        Rushing,
        Hit,
        Whiff
    }

    [Header("Rush")]
    [Min(0f)] public float rushDistance = 5f;
    [Min(0.01f)] public float rushSpeed = 20f;

    [Header("Collision")]
    public LayerMask opponentLayers;
    [Min(0f)] public float hitRadius = 0.5f;
    public Vector3 hitboxOffset = new Vector3(0f, 1f, 0f);

    [Header("Animations")]
    public string rushAnimation = "SideSpecial";
    public string hitAnimation = "SideSpecialHit";
    public string whiffAnimation = "SideSpecialWhiff";

    [Header("Recovery")]
    [Min(0f)] public float hitDuration = 0.5f;
    [Min(0f)] public float whiffDuration = 0.8f;

    private FighterStateMachine fsm;
    private Phase phase;

    private float distanceTravelled;
    private float phaseTimer;
    private bool originalGravityActive;
    private bool physicsStepPending;

    public override bool CanExit => false;
    public override bool ControlsAirMovement => true;

    public override void OnAttackEnter(FighterStateMachine f, AttackObject attack)
    {
        fsm = f;
        distanceTravelled = 0f;
        phaseTimer = 0f;
        phase = Phase.Rushing;
        physicsStepPending = false;

        originalGravityActive = fsm.physics.useGravity;
        fsm.physics.useGravity = false;

        fsm.physics.SetVelocity(new Vector3(fsm.direction * rushSpeed, 0f, 0f));

        if (!string.IsNullOrEmpty(rushAnimation))
            fsm.animator.PlayAnimation(rushAnimation);
    }

    public override void OnAttackUpdate(FighterStateMachine f, AttackObject attack)
    {
        if (phase == Phase.Rushing)
        {
            if (CheckForOpponent())
                BeginHit();

            return;
        }

        phaseTimer += Time.deltaTime;

        if (phase == Phase.Hit && phaseTimer >= hitDuration)
            FinishAttack();

        if (phase == Phase.Whiff && phaseTimer >= whiffDuration)
            FinishAttack();
    }

    public override void OnAttackFixedUpdate(FighterStateMachine f, AttackObject attack)
    {
        if (phase != Phase.Rushing)
            return;

        // Consume the movement result from the previous physics tick.
        if (physicsStepPending)
        {
            distanceTravelled += Mathf.Abs(fsm.physics.LastResolvedMovement.x);
            physicsStepPending = false;

            if (CheckForOpponent())
            {
                BeginHit();
                return;
            }

            if (fsm.physics.LastHitWall || fsm.physics.LastHitCeiling ||
                distanceTravelled >= rushDistance - 0.001f)
            {
                BeginWhiff();
                return;
            }
        }

        float remainingDistance = rushDistance - distanceTravelled;

        if (remainingDistance <= 0f)
        {
            BeginWhiff();
            return;
        }

        float stepDistance = Mathf.Min(rushSpeed * Time.fixedDeltaTime, remainingDistance);

        fsm.physics.SetHorizontalVelocity(fsm.direction * stepDistance / Time.fixedDeltaTime);
        physicsStepPending = true;
    }

    private bool CheckForOpponent()
    {
        Vector3 center = fsm.transform.position + hitboxOffset;

        Collider[] hits = Physics.OverlapSphere(
            center,
            hitRadius,
            opponentLayers,
            QueryTriggerInteraction.Collide
        );

        foreach (Collider col in hits)
        {
            if (col == null)
                continue;

            FighterStateMachine opponent = col.GetComponentInParent<FighterStateMachine>();

            if (opponent == null || opponent == fsm)
                continue;

            if (!opponent.gameObject.activeInHierarchy)
                continue;

            return true;
        }

        return false;
    }

    private void BeginHit()
    {
        if (phase != Phase.Rushing)
            return;

        phase = Phase.Hit;
        phaseTimer = 0f;

        RestoreGravity();

        if (!string.IsNullOrEmpty(hitAnimation))
            fsm.animator.PlayAnimation(hitAnimation);
    }

    private void BeginWhiff()
    {
        if (phase != Phase.Rushing)
            return;

        phase = Phase.Whiff;
        phaseTimer = 0f;

        RestoreGravity();

        if (!string.IsNullOrEmpty(whiffAnimation))
            fsm.animator.PlayAnimation(whiffAnimation);

        if (whiffDuration <= 0f)
            FinishAttack();
    }

    private void RestoreGravity()
    {
        if (fsm == null || fsm.physics == null)
            return;

        fsm.physics.useGravity = originalGravityActive;

        if (fsm.ecb != null && fsm.ecb.IsGrounded && fsm.physics.Velocity.y < 0f)
            fsm.physics.SetVerticalVelocity(0f);

        // Stop the rush when entering hit or whiff recovery.
        fsm.physics.SetHorizontalVelocity(0f);
    }

    private void FinishAttack()
    {
        if (fsm == null || !(fsm.currentState is AttackState))
            return;

        RestoreGravity();

        if (fsm.ecb != null && fsm.ecb.IsGrounded)
            fsm.SetState(new IdleState());
        else
            fsm.SetState(new AirState());
    }

    public override void OnAttackExit(FighterStateMachine f, AttackObject attack)
    {
        RestoreGravity();
    }
}
