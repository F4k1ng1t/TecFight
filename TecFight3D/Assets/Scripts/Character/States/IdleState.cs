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
        fsm.animator?.PlayAnimation("Idle");
    }
    public void Exit()
    {

    }
    public void Update()
    {
        walkHeld = fsm.Walk(walkHeld);
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
        fsm.GroundedActionable();
        
        
    }
    public void FixedUpdate()
    {

    }
}
