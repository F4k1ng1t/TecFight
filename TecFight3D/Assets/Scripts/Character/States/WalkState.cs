
using UnityEngine;

public class WalkState : IFighterState
{
    private FighterStateMachine fsm;
    private CharacterObject co;
    private FighterInput fi;
    private FighterInputProcesser fip;
    private FighterECB ecb;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = f.charObj;
        fi = f.input;
        fip = f.fip;
        ecb = f.GetComponent<FighterECB>();

        fsm.animator?.PlayAnimation("Walk", true, 0);
    }

    public void Exit() { }

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

        if (fi.Shield)
        {
            fsm.SetState(new ShieldState());
        }
    }

    public void FixedUpdate()
    {
        if (ecb == null) return;

        float moveX = fi.MoveInput.x * co.walkSpeed;

        // Store horizontal momentum for jumps and air drift.
        fsm.gravity.horizontalVelocity = moveX;

        Vector3 displacement = new Vector3(
            moveX * Time.fixedDeltaTime,
            0f,
            0f
        );

        ecb.Move(displacement);
    }
}
