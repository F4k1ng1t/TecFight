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
        if (fip.JumpPressed() && fsm.currentDJC > 0)
        {
            fsm.SetState(new DoubleJumpState());
            return;
        }

        float currentVelocity = rig.linearVelocity.x;

        if (Mathf.Abs(currentVelocity) < co.airSpeed)
        {
            rig.AddForce(fi.MoveInput.x * co.airAccel, 0, 0, ForceMode.Acceleration);

            rig.linearVelocity = new Vector3(Mathf.Clamp(rig.linearVelocity.x, -co.airSpeed, co.airSpeed), rig.linearVelocity.y, rig.linearVelocity.z);
        }
        else if (fip.AttemptMoveOpposite())
        {
            rig.linearVelocity = new Vector3(Mathf.Clamp(currentVelocity, -co.airSpeed, co.airSpeed), rig.linearVelocity.y, rig.linearVelocity.z);

            rig.AddForce(fi.MoveInput.x * co.airAccel, 0, 0, ForceMode.Acceleration);
        }
        else
        {
            rig.linearVelocity = new Vector3(currentVelocity, rig.linearVelocity.y, rig.linearVelocity.z);
        }

        if (fip.FastFall())
        {
            rig.AddForce(0, -co.fastfallAccel, 0, ForceMode.Acceleration);

            rig.linearVelocity = new Vector3(rig.linearVelocity.x, Mathf.Max(rig.linearVelocity.y, -co.fastfallSpeed), rig.linearVelocity.z);
        }
    }
}