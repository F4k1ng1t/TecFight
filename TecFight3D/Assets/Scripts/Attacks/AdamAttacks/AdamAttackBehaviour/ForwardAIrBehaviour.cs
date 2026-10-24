using UnityEngine;

[CreateAssetMenu(fileName = "ForwardAirBehaviour", menuName = "Attack Behaviours/Forward Air")]
public class ForwardAirBehaviour : AttackBehaviour
{
    public float recoilSpeed = 8f;
    public float recoilDuration = 0.15f;

    private float recoilTimer;
    private bool recoiling;

    public override void OnAttackEnter(FighterStateMachine fsm, AttackObject attack)
    {
        recoilTimer = 0f;
        recoiling = false;
    }

    public override void OnRecoil(FighterStateMachine fsm, AttackObject attack)
    {
        recoilTimer = recoilDuration;
        recoiling = true;
        //fsm.airMovementEnabled = false; // Disable air movement during recoil
    }

    public override void OnAttackFixedUpdate(FighterStateMachine fsm, AttackObject attack)
    {
        if (!recoiling)
            return;

        fsm.ecb.Move(Vector3.left * fsm.direction * recoilSpeed * Time.fixedDeltaTime);

        recoilTimer -= Time.fixedDeltaTime;

        if (recoilTimer <= 0f || fsm.ecb.HitWall)
            recoiling = false;
    }

    public override void OnAttackExit(FighterStateMachine fsm, AttackObject attack)
    {
        recoiling = false;
        //fsm.airMovementEnabled = true; // Re-enable air movement after recoil
    }
}