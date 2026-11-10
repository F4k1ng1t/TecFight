using UnityEngine;

public class AttackAnimationEvents : MonoBehaviour
{
    private FighterStateMachine fsm;

    private void Awake()
    {
        fsm = GetComponentInParent<FighterStateMachine>();
    }
    public void ApplyRecoil()
    {
        if (fsm.currentState is AttackState attackState)
            attackState.ApplyRecoil();
    }
    public void EnableJumpCancel()
    {
        if (fsm.currentState is AttackState attackState)
        {
            if (fsm.ecb.IsGrounded)
            {
                attackState.EnableJumpCancel();
            }
            else if (fsm.currentDJC > 0)
            {
                fsm.currentDJC--;
                attackState.EnableJumpCancel();
            }
            else
            {
                return;
            }
        }
    }

    public void DisableJumpCancel()
    {
        if (fsm.currentState is AttackState attackState)
            attackState.DisableJumpCancel();
    }
}