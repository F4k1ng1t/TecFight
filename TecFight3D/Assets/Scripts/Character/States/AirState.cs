using UnityEngine;

public class AirState : IFighterState
{
    FighterStateMachine fsm;
    float initVelocity;
    
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
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
        if (Mathf.Abs(fsm.input.MoveInput.x) > 0.2f && initVelocity < 5f)
        {
            float currentAirDrift = 15f * fsm.input.MoveInput.x;
            fsm.rig.AddForce(currentAirDrift, 0, 0);
            fsm.rig.linearVelocity = new Vector3(Mathf.Clamp(fsm.rig.linearVelocity.x, -5f, 5f), fsm.rig.linearVelocity.y, 0);
        }
    }
}
