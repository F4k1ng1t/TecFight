
using UnityEngine;

public class AirState : IFighterState
{
    private FighterStateMachine fsm;
    private FighterInput fi;
    private FighterInputProcesser fip;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fi = fsm.input;
        fip = fsm.fip;

        fsm.animator?.PlayAnimation("Fall", true);
    }

    public void Exit() { }

    public void Update()
    {
        if (fip.JumpPressed() && fsm.currentDJC > 0)
        {
            fsm.SetState(new DoubleJumpState());
            return;
        }
        if (fi.Attack)
        {
            fsm.SetStateAttack(fsm.getAttack.GetAttack());
        }
    }

    public void FixedUpdate()
    {
        

        fsm.ApplyAirMovement();
    }
}
