using UnityEngine;

public class AirState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject co;
    FighterInput fi;
    Rigidbody rig;
    FighterInputProcesser fip;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = fsm.charObj;
        fi = fsm.input;
        rig = fsm.rig;
        fip = fsm.fip;

        fsm.animator?.PlayAnimation("Fall", true);
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
        if (fip.Jump())
        {
            fsm.SetState(new DoubleJumpState());
            return;
        }

        // Apply air control while below the speed cap,
        // or when trying to reverse direction above the cap.
        if (Mathf.Abs(rig.linearVelocity.x) < co.airSpeed ||
            fip.CompareCurrentAirDriftMax())
        {
            float airDrift = co.airAccel * fi.MoveInput.x;

            rig.AddForce(
                airDrift,
                0,
                0,
                ForceMode.Acceleration
            );
        }

        if (fip.FastFall())
        {
            rig.AddForce(0, -co.fastfallAccel, 0, ForceMode.Acceleration);

            rig.linearVelocity = new Vector3(rig.linearVelocity.x, Mathf.Max(rig.linearVelocity.y, -co.fastfallSpeed), rig.linearVelocity.z
            );
        }
    }
}