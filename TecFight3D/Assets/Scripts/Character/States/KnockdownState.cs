using UnityEngine;

public class KnockdownState : IFighterState
{
    FighterStateMachine fsm;
    public void Enter(FighterStateMachine f)
    {
        //this handles the animations and hitstun
        // another script does the knockback

        fsm = f;
        fsm.animator.PlayAnimation("KnockdownLand");
    }
    public void Exit()
    {
    }

    public void FixedUpdate()
    {
        if(fsm.input.MoveInput.x > 0.5f)
        {
            //getup roll right
            fsm.SetDirection(1);
            fsm.SetState(new KnockdownRollState());
        }
        if(fsm.input.MoveInput.x < -0.5f)
        {
            //getup roll left
            fsm.SetDirection(-1);
            fsm.SetState(new KnockdownRollState());
        }
        if (fsm.input.Attack || fsm.input.Strong)
        {
            //getup attack
            fsm.SetStateAttack(fsm.getAttack.GetAttack());
        }
        if (fsm.input.MoveInput.y > 0.15f || fsm.input.MoveInput.y < -0.15f)
        {
            //neutral getup
            fsm.SetState(new KnockdownGetupState());
        }
    }

    public void Update()
    {
    }
}
