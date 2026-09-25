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
        rig.AddForce(new Vector3(0, co.doubleJumpForce, 0));
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
