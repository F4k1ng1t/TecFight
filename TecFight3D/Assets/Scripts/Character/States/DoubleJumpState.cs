
using UnityEngine;

public class DoubleJumpState : IFighterState
{
    private FighterStateMachine fsm;
    private CharacterObject co;
    private FighterInputProcesser fip;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = fsm.charObj;
        fip = fsm.fip;

        fsm.gravity.verticalVelocity = co.doubleJumpForce;

        fsm.currentDJC--;

        fsm.SetState(new AirState());
    }

    public void Exit() { }

    public void Update() { }

    public void FixedUpdate() { }
}
