using UnityEngine;

public class AirState : IFighterState
{
    FighterStateMachine fsm;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
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

    }
}
