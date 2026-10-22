using UnityEngine;
//for shield stun specifically but if we can change the stun time we could reuse this
//lowk it might just be easier to make more states

public class StunState : IFighterState
{
    const float stunDuration = 2f;
    private float startTime;
    FighterStateMachine fsm;
    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        //needs its own animation
        f.animator?.PlayAnimation("KnockdownIdle", true, f.direction);
        startTime = Time.time;
    }
    public void Exit()
    {
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        if(Time.time - startTime < stunDuration)
        {
            return;
        }
        fsm.SetState(new IdleState());
    }
}
