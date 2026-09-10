using UnityEngine;

public class JumpState : IFighterState
{
    FighterStateMachine fsm;
    int frames = 0;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        frames = 0;
        fsm.animator.PlayAnimation("JumpSquat", false);
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
        fsm.rig.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        fsm.animator.PlayAnimation("Hop", false);
    }
    public void ShortHop()
    {

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
