
using UnityEngine;

public class WalkState : IFighterState
{
    private FighterStateMachine fsm;
    private CharacterObject co;
    private FighterInput fi;
    private FighterInputProcesser fip;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = f.charObj;
        fi = f.input;
        fip = f.fip;

        fsm.animator?.PlayAnimation("Walk");
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (fip.Idle())
        {
            fsm.SetState(new IdleState());
            return;
        }

        if (fip.IsHoldingJump())
        {
            fsm.SetState(new JumpState());
            return;
        }

        if (fi.Attack)
        {
            fsm.SetStateAttack(fsm.getAttack.GetAttack());
            return;
        }

        if (fi.Strong)
        {
            fsm.SetStateAttack(fsm.getAttack.GetStrong());
            return;
        }

        if (fi.Shield)
        {
            fsm.SetState(new ShieldState());
            return;
        }

        if (fi.Special)
        {
            fsm.SetStateAttack(fsm.getAttack.GetSpecial());
            return;
        }
    }

    public void FixedUpdate()
    {
        if (fsm.physics == null)
            return;

        float inputX = fi.MoveInput.x;
        float moveX = inputX * co.walkSpeed;

        if (inputX > 0f)
            fsm.SetDirection(1);
        else if (inputX < 0f)
            fsm.SetDirection(-1);

        fsm.physics.SetHorizontalVelocity(moveX);
    }
}
