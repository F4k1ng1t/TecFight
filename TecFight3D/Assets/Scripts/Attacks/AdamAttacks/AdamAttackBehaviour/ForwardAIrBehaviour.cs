
using UnityEngine;

[CreateAssetMenu(fileName = "ForwardAirBehaviour", menuName = "Attack Behaviours/Forward Air")]
public class ForwardAirBehaviour : AttackBehaviour
{
    public float recoilSpeed = 8f;
    public float recoilDuration = 0.15f;

    private float recoilTimer;
    private bool recoiling;
    private float savedHorizontalVelocity;

    public override void OnAttackEnter(FighterStateMachine fsm, AttackObject attack)
    {
        recoilTimer = 0f;
        recoiling = false;
    }

    public override void OnRecoil(FighterStateMachine fsm, AttackObject attack)
    {
        recoilTimer = recoilDuration;
        recoiling = true;

        savedHorizontalVelocity = fsm.physics.Velocity.x;
        fsm.physics.SetHorizontalVelocity(-fsm.direction * recoilSpeed);
    }

    public override void OnAttackFixedUpdate(FighterStateMachine fsm, AttackObject attack)
    {
        if (!recoiling)
            return;

        fsm.physics.SetHorizontalVelocity(-fsm.direction * recoilSpeed);

        recoilTimer -= Time.fixedDeltaTime;

        if (recoilTimer <= 0f || fsm.physics.LastHitWall)
        {
            recoiling = false;
            fsm.physics.SetHorizontalVelocity(savedHorizontalVelocity);
        }
    }

    public override void OnAttackExit(FighterStateMachine fsm, AttackObject attack)
    {
        recoiling = false;
    }
}
