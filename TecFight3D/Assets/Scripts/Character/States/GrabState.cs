
using UnityEngine;

public class GrabState : IFighterState
{
    private FighterStateMachine fsm;
    private GrabInteraction interaction;

    public GrabState(GrabInteraction grabInteraction)
    {
        interaction = grabInteraction;
    }

    public void Enter(FighterStateMachine f)
    {
        fsm = f;

        fsm.physics.ClearVelocity();
        interaction.PlayGrabAnimation();
    }

    public void Update()
    {
        if (!interaction.IsGrabbing)
            return;

        // Grab inputs and throw selection can go here later.
    }

    public void FixedUpdate()
    {
        if (interaction.IsGrabbing)
            fsm.physics.ClearVelocity();
    }

    public void Exit()
    {
    }
}
