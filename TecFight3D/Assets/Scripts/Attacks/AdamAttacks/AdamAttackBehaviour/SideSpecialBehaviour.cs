
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

    public override bool CanExit => false;
    public override bool ControlsAirMovement => true;

    public override void OnAttackEnter(FighterStateMachine f, AttackObject attack)
    {
        fsm = f;
        distanceTravelled = 0f;
        phaseTimer = 0f;
        phase = Phase.Rushing;

        originalGravityActive = fsm.gravity != null && fsm.gravity.active;

        if (fsm.gravity != null)
            fsm.gravity.active = false;

        if (!string.IsNullOrEmpty(rushAnimation))
            fsm.animator.PlayAnimation(rushAnimation);
    }

    public override void OnAttackUpdate(FighterStateMachine f, AttackObject attack)
    {
        if (phase == Phase.Rushing)
        {
            if (CheckForOpponent())
            {
                BeginHit();
            }

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

        float remainingDistance = rushDistance - distanceTravelled;

        if (remainingDistance <= 0f)
        {
            BeginWhiff();
            return;
        }

        float stepDistance = Mathf.Min(
            rushSpeed * Time.fixedDeltaTime,
            remainingDistance
        );

        Vector3 displacement = new Vector3(
            fsm.direction * stepDistance,
            0f,
            0f
        );

        Vector3 resolvedMovement = fsm.ecb.Move(displacement);

        distanceTravelled += Mathf.Abs(resolvedMovement.x);

        if (CheckForOpponent())
        {
            BeginHit();
            return;
        }

        if (fsm.ecb.HitWall || fsm.ecb.HitCeiling)
            BeginWhiff();
        else if (distanceTravelled >= rushDistance - 0.001f)
            BeginWhiff();
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

            FighterStateMachine opponent =
                col.GetComponentInParent<FighterStateMachine>();

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
        if (fsm.gravity != null)
            fsm.gravity.active = originalGravityActive;

        if (fsm.ecb != null && fsm.ecb.IsGrounded && fsm.gravity != null)
            fsm.gravity.verticalVelocity = 0f;
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
