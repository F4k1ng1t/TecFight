using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InitialDashState : IFighterState
{
    FighterStateMachine fsm;

    private int frames;
    private int initDirection;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        frames = 0;

        // Direction of the dash when we entered.
        initDirection = fsm.direction;
        fsm.animator.PlayAnimation("Dash", true, 0);
    }

    public void Exit()
    {
    }

    public void Update()
    {

        if (fsm.input.IsHoldingJump)
        {
            fsm.SetState(new JumpState());
            return;
        }
        if (fsm.input.Flick)
        {
            float flickX = fsm.input.FlickDirection.x;

            // Horizontal flick opposite our current direction.
            if (Mathf.Abs(flickX) > 0.5f &&
                Mathf.Sign(flickX) == -initDirection)
            {
                fsm.input.ConsumeFlick();

                fsm.FlipDirection();
                fsm.SetState(new InitialDashState());

                return;
            }
        }
    }

    public void FixedUpdate()
    {
        fsm.rig.linearVelocity = new Vector3(fsm.direction * fsm.charObj.initialDashSpeed, fsm.rig.linearVelocity.y, 0f);

        frames++;
        if (frames == 20)
        {
            if (Mathf.Abs(fsm.input.MoveInput.x) > 0.2f)
            {
                if (Mathf.Sign(fsm.rig.linearVelocity.x) != Mathf.Sign(fsm.input.MoveInput.x))
                {
                    fsm.SetState(new RunTurnaroundState());
                }
                else
                {
                    fsm.SetState(new RunState());
                }
                    
            }
            else
            {
                fsm.rig.linearVelocity = new Vector3(0f, fsm.rig.linearVelocity.y, 0f);
                fsm.SetState(new IdleState());
            }
        }
    }
}