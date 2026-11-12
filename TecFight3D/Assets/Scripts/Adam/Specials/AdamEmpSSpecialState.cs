using UnityEngine;
//this is like all duplicate code
public class AdamEmpSSpecialState : IFighterState
{
    private const int rushStart = 8;
    private const int hitboxStop = 25;
    private const int rushStop = 35;
    private const float rushDistance = 10f;
    private const float hitboxRadius = 0.5f;
    private const int hurtboxMask = 1<<7;

    private int frame = 0;
    private Vector3 offset = new Vector3(0, 1, 0);
    private FighterStateMachine fsm;
    private bool hit = false;
    private bool jumpCancelEnabled;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator.PlayAnimation("EmpoweredSSpecialChase");
        fsm.physics.SetVelocity(Vector3.zero);
        fsm.physics.useGravity = false;
        DisableJumpCancel();
    }

    public void Exit()
    {
        fsm.physics.useGravity = true;
    }

    public void Update()
    {

        if (jumpCancelEnabled && fsm.fip.JumpPressed())
        {
            fsm.SetState(new JumpState());
            return;
        }

        if (fsm.animator.IsPlaying())
            return;

        if (!fsm.ecb.IsGrounded)//the person is in the air
            fsm.SetState(new AirState());
        else
            fsm.SetState(new IdleState());
    }

    public void FixedUpdate()
    {
        frame++;
        if (frame == rushStop)
        {
            fsm.physics.useGravity = true;
        }
        if (frame > rushStop || frame < rushStart || hit)
        {
            return;
        }
        fsm.ecb.Move(Vector3.right * (rushDistance / (rushStop - rushStart)) * fsm.direction);
        if (frame < hitboxStop)
        {
            Collider[] hitHurtboxes = Physics.OverlapSphere(fsm.transform.position + offset, hitboxRadius, hurtboxMask, QueryTriggerInteraction.Collide);
            if (hitHurtboxes.Length > 0)
            {
                foreach (Collider hurtbox in hitHurtboxes)
                {
                    if (hurtbox.transform.root == fsm.transform.root)
                        continue;
                    fsm.animator.PlayAnimation("EmpoweredSSpecialHit"); //this is probably going to need some hitbox witchcraft
                    hit = true;
                }
            }
        }
    }

    public void ApplyRecoil()
    {
    }

    public void EnableJumpCancel()
    {
        jumpCancelEnabled = true;
    }

    public void DisableJumpCancel()
    {
        jumpCancelEnabled = false;
    }
}
