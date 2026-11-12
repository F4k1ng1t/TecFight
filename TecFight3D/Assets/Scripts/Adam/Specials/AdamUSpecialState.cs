using UnityEngine;
public class AdamUSpecialState : IFighterState
{
    private const int maxChargeFrames = 55;
    private const float travelDistancePerFrame = 12f/60f;

    private FighterStateMachine fsm;
    private int frame = 0;
    private int chargeFrame = 5;
    private bool launch = false;
    private Vector2 launchDirection;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator.PlayAnimation("USpecial_Charge");
        fsm.physics.SetVelocity(Vector3.zero);
        fsm.physics.useGravity = false;
    }

    public void Exit()
    {
        fsm.physics.useGravity = true;
        fsm.animator.transform.localRotation = Quaternion.identity;
    }

    public void Update()
    {
        if (!launch && !fsm.input.Special)
        {
            launch = true;
            fsm.animator.PlayAnimation("USpecial_Launch");
        }
        if (!launch)
        {
            launchDirection = fsm.input.MoveInput;
            fsm.animator.transform.localRotation = Quaternion.Euler(0, 0,
                Mathf.Atan2(fsm.input.MoveInput.y, fsm.input.MoveInput.x) * Mathf.Rad2Deg - 90);
        }

        if (fsm.animator.IsPlaying())
            return;

        fsm.SetState(new SpecialFallState());
    }

    public void FixedUpdate()
    {
        frame++;
        //charging
        if(!launch)
        {
            chargeFrame++;
            if (chargeFrame == maxChargeFrames)
            {
                launch = true;
                fsm.animator.PlayAnimation("USpecial_Launch");
            }
            return;
        }
        //launch
        if (launch && frame < chargeFrame * 2)
        {
            fsm.ecb.Move(launchDirection * travelDistancePerFrame);
            return;
        }
        fsm.SetState(new SpecialFallState());
    }
}
