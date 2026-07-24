using UnityEngine;

public class RunTurnaroundState : IFighterState
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
        fsm.FlipDirection();
    }
    public void Update()
    {
        if (fsm.input.JumpPressed)
        {
            Debug.Log("bruh");
            fsm.SetState(new JumpState());
        }
    }
    public void FixedUpdate()
    {
        frames++;
        fsm.rig.linearVelocity = new Vector3(fsm.direction * 3f, fsm.rig.linearVelocity.y, 0);
        if (frames == 20)
        {
            fsm.SetState(new RunState());
        }
        
    }
}
