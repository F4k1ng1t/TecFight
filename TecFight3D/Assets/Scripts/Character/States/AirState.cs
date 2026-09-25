using UnityEngine;

public class AirState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject co;
    FighterInput fi;
    Rigidbody rig;
    FighterInputProcesser fip;
    float initVelocity;
    float currentAirDrift = 0;
    
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = fsm.charObj;
        fi = fsm.input;
        rig = fsm.rig;
        fip = fsm.fip;
        initVelocity = fsm.rig.linearVelocity.x;
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
        }
        if (fip.CompareCurrentAirDriftMax())
        {
            currentAirDrift = co.airAccel * fi.MoveInput.x;
            rig.AddForce(currentAirDrift, 0, 0);
        }
        else
        {
            currentAirDrift = co.airAccel * fi.MoveInput.x;
            rig.AddForce(currentAirDrift, 0, 0, ForceMode.Acceleration);
            rig.linearVelocity = new Vector3(Mathf.Clamp(rig.linearVelocity.x, -5f, 5f), rig.linearVelocity.y, 0);
        }
        if (fip.FastFall())
        {
                //Debug.Log("Why are you running");
            rig.AddForce(0, -co.fastfallAccel, 0, ForceMode.Acceleration);
            rig.linearVelocity = new Vector3(rig.linearVelocity.x, Mathf.Max(rig.linearVelocity.y, -co.fastfallSpeed));
        }
    }
}
