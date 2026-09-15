using UnityEngine;

public class AirState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject c;
    FighterInput i;
    Rigidbody rb;
    float initVelocity;
    
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        c = fsm.charObj;
        i = fsm.input;
        rb = fsm.rig;
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
        if (Mathf.Abs(rb.linearVelocity.x) >= c.airSpeed)
        {
            if(Mathf.Sign(i.MoveInput.x) != Mathf.Sign(rb.linearVelocity.x) && i.MoveInput.x != 0)
            {
                currentAirDrift = c.airAccel * i.MoveInput.x;
                rb.AddForce(currentAirDrift, 0, 0);
                //rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x, -5f, 5f), rb.linearVelocity.y, 0);
            }
        }
        else
        {
            currentAirDrift = c.airAccel * i.MoveInput.x;
            rb.AddForce(currentAirDrift, 0, 0);
            rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x, -5f, 5f), rb.linearVelocity.y, 0);
        }
        if (rb.linearVelocity.y <= 0)
        {
            //fastfalling
        }
    }
}
