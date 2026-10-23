
using UnityEngine;

public class AttackState : IFighterState
{
    private FighterStateMachine fsm;
    private AttackObject ao;
    private AttackBehaviour activeBehaviour;
    private bool jumpCancelEnabled;

    public void Enter(FighterStateMachine f, AttackObject a)
    {
        fsm = f;
        ao = a;
        DisableJumpCancel();

        if (fsm == null || ao == null)
        {
            Debug.LogError("AttackState.Enter received a null FighterStateMachine or AttackObject.", fsm);
            return;
        }

        activeBehaviour = ao.attackBehaviour != null
            ? UnityEngine.Object.Instantiate(ao.attackBehaviour)
            : null;

        fsm.animator.PlayAnimation(ao.animationName);
        activeBehaviour?.OnAttackEnter(fsm, ao);
    }

    public void Exit()
    {
        DisableJumpCancel();
        activeBehaviour?.OnAttackExit(fsm, ao);

        if (activeBehaviour != null)
            UnityEngine.Object.Destroy(activeBehaviour);

        activeBehaviour = null;
    }

    public void Update()
    {
        if (fsm == null || ao == null)
            return;

        activeBehaviour?.OnAttackUpdate(fsm, ao);

        if (fsm.currentState != this)
            return;

        if (jumpCancelEnabled && fsm.fip != null && fsm.fip.JumpPressed())
        {
            jumpCancelEnabled = false;
            fsm.SetState(new JumpState());
            return;
        }

        if (fsm.animator != null && fsm.animator.IsPlaying())
            return;

        if (activeBehaviour != null && !activeBehaviour.CanExit)
            return;

        if (ao.aerial)
            fsm.SetState(new AirState());
        else
            fsm.SetState(new IdleState());
    }

    public void FixedUpdate()
    {
        if (fsm == null || ao == null)
            return;

        if (ao.aerial && !(activeBehaviour?.ControlsAirMovement ?? false))
            fsm.ApplyAirMovement();

        activeBehaviour?.OnAttackFixedUpdate(fsm, ao);
    }

    public void ApplyRecoil()
    {
        if (fsm == null || ao == null)
            return;

        activeBehaviour?.OnRecoil(fsm, ao);
    }

    public void EnableJumpCancel()
    {
        jumpCancelEnabled = true;
    }

    public void DisableJumpCancel()
    {
        jumpCancelEnabled = false;
    }
}
