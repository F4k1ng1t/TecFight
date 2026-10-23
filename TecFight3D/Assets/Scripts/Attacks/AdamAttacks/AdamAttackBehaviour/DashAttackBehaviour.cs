
using UnityEngine;

[CreateAssetMenu(fileName = "DashAttackBehaviour", menuName = "Attack Behaviours/DashAttack")]
public class DashAttackBehaviour : AttackBehaviour
{
    public float dashSpeed = 8f;
    public float dashDuration = 0.2f;

    private float elapsedTime;
    private bool isDashing;

    public override void OnAttackEnter(FighterStateMachine fsm, AttackObject attack)
    {
        elapsedTime = 0f;
        isDashing = true;

        Debug.Log("Dash started!");
    }

    public override void OnAttackFixedUpdate(FighterStateMachine fsm, AttackObject attack)
    {
        if (!isDashing)
            return;

        elapsedTime += Time.fixedDeltaTime;

        float deltaX = dashSpeed * fsm.direction * Time.fixedDeltaTime;

        fsm.ecb.Move(Vector3.right * deltaX);

        if (fsm.ecb.HitWall)
        {
            isDashing = false;
            return;
        }

        if (elapsedTime >= dashDuration)
            isDashing = false;
    }

    public override void OnAttackExit(FighterStateMachine fsm, AttackObject attack)
    {
        isDashing = false;
        Debug.Log("Dash ended!");
    }
}
