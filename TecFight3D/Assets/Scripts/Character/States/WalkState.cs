using UnityEngine;

public class WalkState : IFighterState
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
        fsm.rig.linearVelocity = new Vector3(fsm.input.MoveInput.x * 5f, fsm.rig.linearVelocity.y, 0);
        if (fsm.input.Smash)
        {
            fsm.SetDirection(fsm.input.MoveInput.x > 0);
            fsm.SetState(new InitialDashState());
        }
        else if (fsm.input.MoveInput.x == 0)
        {
            fsm.rig.linearVelocity = new Vector3(0, fsm.rig.linearVelocity.y, 0);
            fsm.SetState(new IdleState());
        }
        if (fsm.input.JumpPressed)
        {
            fsm.SetState(new JumpState());
        }

    }
    public void FixedUpdate()
    {

    }
}
