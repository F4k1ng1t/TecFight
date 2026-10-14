using UnityEngine;

public class AirState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject co;
    FighterInput fi;
    Rigidbody rig;
    FighterInputProcesser fip;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = fsm.charObj;
        fi = fsm.input;
        rig = fsm.rig;
        fip = fsm.fip;

        fsm.animator?.PlayAnimation("Fall", true);
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (fi.Attack)
        {
            fsm.SetStateAttack(fsm.getAttack.GetAttack());
        }
    }

    public void FixedUpdate()
    {
        if (fip.JumpPressed() && fsm.currentDJC > 0)
        {
            fsm.SetState(new DoubleJumpState());
            return;
        }
        if (fi.Attack)
        {
            fsm.getAttack.GetAttack();
        }
        fsm.ApplyAirMovement();
        
    }
}