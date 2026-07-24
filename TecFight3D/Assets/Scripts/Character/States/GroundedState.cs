using UnityEngine;

public class GroundedState : IFighterState
{
    FighterStateMachine fsm;
    int frames = 0;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        frames = 0;
    }
    public void Exit()
    {

    }
    public void Update()
    {
        
    }
    public void FixedUpdate()
    {
        frames++;
        if (frames == 4)
        {
            fsm.SetState(new IdleState());
        }
    }
}
