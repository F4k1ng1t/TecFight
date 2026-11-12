using UnityEngine;

public class AdamEmpUSpecialState: IFighterState
{
    private const int maxChargeFrames = 20;
    private const float travelDistancePerFrame = 24f / 60f;

    private FighterStateMachine fsm;
    private int frame = 0;
    private bool launch = false;
    private Vector2 launchDirection;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator.PlayAnimation("EmpoweredUSpecial_Charge");
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
            fsm.animator.PlayAnimation("EmpoweredUSpecial_Launch");
        }
        if (!launch)
        {
            launchDirection = fsm.input.MoveInput;
            fsm.animator.transform.localRotation = Quaternion.Euler(0, 0,
                Mathf.Atan2(fsm.input.MoveInput.y, fsm.input.MoveInput.x) * Mathf.Rad2Deg - 90);
            return;
        }

        if (fsm.animator.IsPlaying())
            return;

        //fsm.SetState(new SpecialFallState());
    }

    public void FixedUpdate()
    {
        if (!launch)
        {
            return;
        }
        frame++;
        //launch
        if (launch && frame < maxChargeFrames)
        {
            fsm.ecb.Move(launchDirection * travelDistancePerFrame);
            return;
        }
        fsm.SetState(new SpecialFallState());
    }
}
