
using UnityEngine;

public class SpecialFallState : IFighterState
{
    private FighterStateMachine fsm;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;

        fsm.animator?.PlayAnimation("SpecialFall");
    }

    public void Exit() { }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
        if (fsm == null)
            return;

        fsm.ApplyAirMovement();
    }
}
