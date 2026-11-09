public class AdamEmpUSpecialState: IFighterState
{
    private FighterStateMachine fsm;
    private bool jumpCancelEnabled;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator.PlayAnimation("EmpoweredUSpecial_Charge");
        DisableJumpCancel();
    }

    public void Exit()
    {
        DisableJumpCancel();
    }

    public void Update()
    {

        if (jumpCancelEnabled && fsm.fip.JumpPressed())
        {
            fsm.SetState(new JumpState());
            return;
        }

        if (fsm.animator.IsPlaying())
            return;

        if (!fsm.ecb.IsGrounded)//the person is in the air
            fsm.SetState(new AirState());
        else
            fsm.SetState(new IdleState());
    }

    public void FixedUpdate()
    {
    }

    public void ApplyRecoil()
    {
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
