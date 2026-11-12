
using UnityEngine;

public class GrabbedState : IFighterState
{
    private FighterStateMachine fsm;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;

        fsm.physics.ClearVelocity();
        fsm.animator?.PlayAnimation("Grabbed");
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
        fsm.physics.ClearVelocity();
    }

    public void Exit()
    {
    }
}
