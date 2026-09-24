using UnityEngine;

public class AirState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject co;
    FighterInput fi;
    Rigidbody rig;
    float initVelocity;
    
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = fsm.charObj;
        fi = fsm.input;
        rig = fsm.rig;
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
        float currentAirDrift = 0;
        if (Mathf.Abs(rig.linearVelocity.x) >= co.airSpeed)
        {
            if(Mathf.Sign(fi.MoveInput.x) != Mathf.Sign(rig.linearVelocity.x) && fi.MoveInput.x != 0)
            {
                currentAirDrift = co.airAccel * fi.MoveInput.x;
                rig.AddForce(currentAirDrift, 0, 0);
                //rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x, -5f, 5f), rb.linearVelocity.y, 0);
            }
        }
        else
        {
            currentAirDrift = co.airAccel * fi.MoveInput.x;
            rig.AddForce(currentAirDrift, 0, 0, ForceMode.Acceleration);
            rig.linearVelocity = new Vector3(Mathf.Clamp(rig.linearVelocity.x, -5f, 5f), rig.linearVelocity.y, 0);
        }
        if (rig.linearVelocity.y <= 0)
        {
            if(fi.MoveInput.y < -0.5f)
            {
                Debug.Log("Why are you running");
                rig.AddForce(0, -c.fastfallAccel, 0, ForceMode.Acceleration);
                rig.linearVelocity = new Vector3(rig.linearVelocity.x, Mathf.Max(rig.linearVelocity.y, -co.fastfallSpeed));
            }
        }
    }
}
