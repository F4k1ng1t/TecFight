using UnityEngine;

public class DoubleJumpState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject co;
    Rigidbody rig;
    FighterInputProcesser fip;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = fsm.charObj;
        rig = fsm.rig;
        fip = fsm.fip;
        fsm.gravity.active = false;
        rig.linearVelocity = new Vector3(Mathf.Clamp(rig.linearVelocity.x, -co.airSpeed, co.airSpeed), co.doubleJumpForce, rig.linearVelocity.z);
        fsm.gravity.active = true;
        fsm.currentDJC--;
        fsm.SetState(new AirState());
    }
    public void Exit()
    {

    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
}
