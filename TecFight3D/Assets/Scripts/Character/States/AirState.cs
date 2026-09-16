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
        initVelocity = fsm.rig.linearVelocity.x;
        fsm.animator.PlayAnimation("Fall", true);
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
            rb.AddForce(currentAirDrift, 0, 0, ForceMode.Acceleration);
            rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x, -5f, 5f), rb.linearVelocity.y, 0);
        }
        if (rb.linearVelocity.y <= 0)
        {
            if(i.MoveInput.y < -0.5f)
            {
                Debug.Log("Why are you running");
                rb.AddForce(0, -c.fastfallAccel, 0, ForceMode.Acceleration);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -c.fastfallSpeed));
            }
        }
    }
}
