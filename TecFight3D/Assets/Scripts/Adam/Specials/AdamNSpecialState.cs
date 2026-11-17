public class AdamNSpecialState : IFighterState
{
    private FighterStateMachine fsm;
    private bool jumpCancelEnabled;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator.PlayAnimation("NSpecial");
        EnableJumpCancel();
    }

    public void Exit()
    {
        DisableJumpCancel();
    }

    public void Update()
    {
        if (jumpCancelEnabled && fsm.fip.JumpPressed())
        {
            if (fsm.ecb.IsGrounded)
            {
                fsm.SetState(new JumpState());
                return;
            }
            else if (!fsm.ecb.IsGrounded && fsm.currentDJC > 0)
            {
                fsm.SetState(new DoubleJumpState());
                return;
            }
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
