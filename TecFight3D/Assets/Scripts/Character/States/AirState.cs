
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

        fsm.animator?.PlayAnimation("Fall");
    }

    public void Exit() { }

    public void Update()
    {
        if (fsm == null || fi == null || fip == null)
            return;

        if (fip.JumpPressed() && fsm.currentDJC > 0)
        {
            fsm.SetState(new DoubleJumpState());
            return;
        }

        if (fi.Attack)
        {
            AttackObject attack = fsm.getAttack.GetAttack();

            if (attack != null)
                fsm.SetStateAttack(attack);

            return;
        }

        if (fi.Special)
        {
            AttackObject special = fsm.getAttack.GetSpecial();

            Debug.Log(special == null
                ? "GetSpecial returned NULL"
                : $"Selected special: {special.attackName}");

            if (special != null)
                fsm.SetStateAttack(special);

            return;
        }
        if (fi.Shield)
        {
            fsm.SetState(new AirdodgeState());
        }
    }

    public void FixedUpdate()
    {
        if (fsm == null)
            return;

        fsm.ApplyAirMovement();
    }
}
