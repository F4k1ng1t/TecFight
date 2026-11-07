using UnityEngine;

public class WavelandState : IFighterState
{
    private FighterStateMachine fsm;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;

        fsm.physics.SetHorizontalVelocity(0f);
        fsm.physics.SetVerticalVelocity(0f);

        fsm.animator?.PlayAnimation("DodgeLand");
    }

    public void Update()
    {
        if (fsm.animator != null && fsm.animator.IsAnimationFinished("DodgeLand"))
            fsm.SetState(new IdleState());
    }

    public void FixedUpdate()
    {
    }

    public void Exit()
    {
    }
}