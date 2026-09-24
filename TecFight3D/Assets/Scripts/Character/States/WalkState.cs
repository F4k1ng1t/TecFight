using UnityEngine;

public class WalkState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject co;
    FighterInput fi;
    FighterInputProcesser fip;
    Rigidbody rig;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = f.charObj;
        fi = f.input;
        fip = f.fip;
        rig = f.rig;
        fsm.animator.PlayAnimation("Walk", true, 0);
    }
    public void Exit()
    {

    }
    public void Update()
    {
        rig.linearVelocity = new Vector3(fi.MoveInput.x * 5f, rig.linearVelocity.y, 0);
        if (fip.Idle())
        {
            rig.linearVelocity = new Vector3(0, rig.linearVelocity.y, 0);
            fsm.SetState(new IdleState());
        }
        if (fip.Jump())
        {
            fsm.SetState(new JumpState());
        }

    }
    public void FixedUpdate()
    {
        rig.linearVelocity = new Vector3(fi.MoveInput.x * 5f, rig.linearVelocity.y, 0);
    }
}
