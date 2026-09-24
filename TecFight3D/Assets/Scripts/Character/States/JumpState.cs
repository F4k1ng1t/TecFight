using UnityEngine;

public class JumpState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject co;
    FighterInputProcesser fip;
    Rigidbody rig;

    int frames = 0;
    const int JUMPSQUAT_LENGTH = 4;
    bool jump_executed = false;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = f.charObj;
        rig = f.rig;
        fip = f.fip;
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
        rig.linearVelocity = new Vector3(rig.linearVelocity.x, 0, rig.linearVelocity.z);
        rig.AddForce(Vector3.up * co.fullhopForce, ForceMode.VelocityChange);
        fsm.animator.PlayAnimation("Hop", false);
    }
    public void ShortHop()
    {
        rig.linearVelocity = new Vector3(rig.linearVelocity.x, 0, rig.linearVelocity.z);
        rig.AddForce(Vector3.up * co.shorthopForce, ForceMode.VelocityChange);
    }
    public void FixedUpdate()
    {
        if (jump_executed) return;
        frames++;
        if(frames > 4)
        {
            jump_executed = true;
            if(fip.Jump())
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
