using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class IdleState : IFighterState
{
    FighterStateMachine fsm;
    CharacterObject co;
    FighterInput fi;
    FighterInputProcesser fip;
    Rigidbody rig;
    int walkHeld = 0;
    int walkTimeThreshold = 75;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = f.charObj;
        fi = f.input;
        fip = f.fip;
        rig = f.rig;
        fsm.animator?.PlayAnimation("Idle", true, fsm.direction);
    }
    public void Exit()
    {

    }
    public void Update()
    {
        if (Mathf.Abs(fi.MoveInput.x) > 0.01f)
        {
            //Debug.Log($"{walkHeld * Time.deltaTime} {walkTimeThreshold * Time.deltaTime}");
            walkHeld++;
        }
        else
        {
            walkHeld = 0;
        }
        if (fi.Flick)
        {
            float flickX = fi.FlickDirection.x;
            if (Mathf.Abs(flickX) > 0.8f)
            {
                fsm.SetDirection(flickX);
                fi.ConsumeFlick();
                fsm.SetState(new InitialDashState());

                return;
            }
        }
        if (fip.Walk() && walkHeld >= 30)
        {
            fsm.SetDirection(fi.MoveInput.x);
            fsm.SetState(new WalkState());
            return;
        }
        if (fip.IsHoldingJump())
        {
            Debug.Log("helo");
            fsm.SetState(new JumpState());
        }
        if (fi.Attack)
        {
            Debug.Log("Jab");
            fsm.SetState(new AttackState());
            fi.Attack = false;
        }
    }
    public void FixedUpdate()
    {

    }
}
