using UnityEngine;

public class TumbleState : IFighterState
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
        if (Mathf.Abs(fsm.input.MoveInput.x) >= 0.2f)
        {
            fsm.SetState(new AirState());
        }
    }
    public void FixedUpdate()
    {
        fsm.ApplyAirMovement();
    }
}
