using UnityEngine;

public class AttackState : IFighterState
{
    FighterStateMachine fsm;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator.PlayAnimation("Jab1", true, 0);
    }
    public void Exit()
    {

    }
    public void Update()
    {
        if (fsm.animator.IsPlaying())
        {
            return;
        }
        //something about input buffers triggering the second jab
        fsm.SetState(new IdleState());
    }
    public void FixedUpdate()
    {

    }
    public void DoAttack(AttackObject attack)
    {
         
    }
}
