
using UnityEngine;

public class JumpState : IFighterState
{
    private FighterStateMachine fsm;
    private CharacterObject co;
    private FighterInputProcesser fip;
    private FighterECB ecb;

    private int frames;
    private const int JUMPSQUAT_LENGTH = 4;
    private bool jumpExecuted;

    private float savedHorizontalVelocity;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = f.charObj;
        fip = f.fip;
        frames = 0;
        fsm.animator.PlayAnimation("JumpSquat");
        ecb = f.GetComponent<FighterECB>();
        
    }
    public void Exit()
    {
        
    }
    public void Update()
    {

        savedHorizontalVelocity = fsm.gravity.horizontalVelocity;

        frames = 0;
        jumpExecuted = false;

        fsm.animator?.PlayAnimation("JumpSquat", false);
    }

    public void Exit() { }

    public void Update() { }

    public void FullHop()
    {
        //fsm.gravity.horizontalVelocity = savedHorizontalVelocity;
        fsm.gravity.horizontalVelocity = Mathf.Clamp(savedHorizontalVelocity, -co.airSpeed, co.airSpeed);
        fsm.gravity.verticalVelocity = co.fullhopForce;
        fsm.animator?.PlayAnimation("Hop");
    }

    public void ShortHop()
    {
        fsm.gravity.horizontalVelocity = savedHorizontalVelocity;
        fsm.gravity.verticalVelocity = co.shorthopForce;
        fsm.animator?.PlayAnimation("Hop");
    }

    public void FixedUpdate()
    {
        if (ecb == null || jumpExecuted)
            return;

        // Keep horizontal movement continuous during jumpsquat.
        ecb.Move(new Vector3(
            savedHorizontalVelocity * Time.fixedDeltaTime,
            0f,
            0f
        ));

        frames++;

        if (frames >= JUMPSQUAT_LENGTH)
        {
            jumpExecuted = true;

            if (fip.IsHoldingJump())
                FullHop();
            else
                ShortHop();

            fsm.SetState(new AirState());
        }
    }
}
