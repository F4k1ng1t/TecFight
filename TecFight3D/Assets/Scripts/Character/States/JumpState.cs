using UnityEngine;

public class JumpState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject c;
    FighterInput i;
    Rigidbody rb;

    int frames = 0;
    const int JUMPSQUAT_LENGTH = 4;
    bool jump_executed = false;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        c = fsm.charObj;
        i = fsm.input;
        rb = fsm.rig;
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
        fsm.rig.AddForce(Vector3.up * c.fullhopForce, ForceMode.VelocityChange);
        fsm.animator.PlayAnimation("Hop", false);
    }
    public void ShortHop()
    {
        fsm.rig.linearVelocity = new Vector3(fsm.rig.linearVelocity.x, 0, fsm.rig.linearVelocity.z);
        fsm.rig.AddForce(Vector3.up * c.shorthopForce, ForceMode.VelocityChange);
    }
    public void FixedUpdate()
    {
        if (jump_executed) return;
        frames++;
        if(frames > 4)
        {
            jump_executed = true;
            if(fsm.input.IsHoldingJump)
            {
                FullHop();
            }
            else
            {
                ShortHop();
            }
        }
    }
}
