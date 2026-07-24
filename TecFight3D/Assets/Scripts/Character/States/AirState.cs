using UnityEngine;

public class AirState : IFighterState
{
    FighterStateMachine fsm;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
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
