using UnityEngine;

public class KnockdownGetupState : IFighterState
{
    const int startDodge = 0;
    const int endDodge = 40;
    private int frame = 0;

    FighterStateMachine fsm;
    
    //i am under the impression that you are intangible when you getup
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator?.PlayAnimation("KnockdownGetup");
    }
    public void Exit()
    {
        fsm.isDodging = false;
    }

    public void FixedUpdate()
    {
        frame++;
        if (!fsm.animator.IsPlaying())
        {
            fsm.SetState(new IdleState());
        }
        if (frame < endDodge)
        {
            fsm.isDodging = false;
            return;
        }
        if (frame < startDodge)
        {
            fsm.isDodging = true;
        }
    }

    void IFighterState.Update()
    {
    }
}
