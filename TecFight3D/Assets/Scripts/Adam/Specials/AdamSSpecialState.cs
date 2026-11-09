using UnityEngine;
public class AdamSSpecialState : IFighterState
{
    private const int rushStart = 8;
    private const int hitboxStop = 25;
    private const int rushStop = 35;
    private const float rushDistance = 7f;
    private const float hitboxRadius = 0.5f;
    private const int hurtboxMask = 1<<7;//binary shift operator is required

    private int frame = 0;
    private Vector3 offset = new Vector3(0, 1, 0);
    private FighterStateMachine fsm;
    private bool hit = false;
    private bool jumpCancelEnabled;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        fsm.animator.PlayAnimation("SSpecialChase");
        DisableJumpCancel();
    }

    public void Exit()
    {
        DisableJumpCancel();
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
        if(frame > rushStop || frame < rushStart || hit)
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
                    Debug.Log("Sspecial hit: " + hurtbox.name);
                    fsm.animator.PlayAnimation("SSpecialHit");
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
