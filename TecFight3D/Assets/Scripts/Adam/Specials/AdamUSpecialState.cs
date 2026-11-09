public class AdamUSpecialState : IFighterState
{
    private const int chargeFrames = 60;
    private const float minTravelDistance = 3f;
    private const float maxTravelDistcane = 12f;
    private const float maxTravelFrames = 60f;

    private FighterStateMachine fsm;
    private bool jumpCancelEnabled;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator.PlayAnimation("USpecial_Charge");
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
