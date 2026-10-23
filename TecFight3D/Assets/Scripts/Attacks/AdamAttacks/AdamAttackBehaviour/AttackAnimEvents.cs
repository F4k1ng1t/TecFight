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
            attackState.EnableJumpCancel();
    }

    public void DisableJumpCancel()
    {
        if (fsm.currentState is AttackState attackState)
            attackState.DisableJumpCancel();
    }
}