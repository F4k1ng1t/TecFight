using UnityEngine;

public class HitState : IFighterState
{
    FighterStateMachine fsm;
    public void Enter(FighterStateMachine f)
    {
        //this handles the animations and hitstun
        // another script does the knockback
        fsm = f;
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    void IFighterState.Update()
    {
        throw new System.NotImplementedException();
    }
}
