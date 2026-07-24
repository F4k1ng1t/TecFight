using UnityEngine;

public class JumpState : IFighterState
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
        
    }
    public void Update()
    {

    }
    public void FullHop()
    {
        fsm.rig.linearVelocity = new Vector3(fsm.rig.linearVelocity.x, 0, fsm.rig.linearVelocity.z);
        fsm.rig.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
    public void FixedUpdate()
    {
        frames++;
        if (frames == 3)
        {
            FullHop();
        }
    }
}
