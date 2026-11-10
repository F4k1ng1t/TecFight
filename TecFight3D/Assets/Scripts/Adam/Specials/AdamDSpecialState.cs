using UnityEngine;

public class AdamDSpecialState : IFighterState
{
    private const float damagePerFrame = 0.1f;
    private const float chargePerFrame = 0.3f;

    private FighterStateMachine fsm;
    private bool jumpCancelEnabled;
    private int walkHeld;
    private KnockbackHandler knockbackHandler;
    private AdamSpecialMeter specialMeter;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        //these should probably be handled differently
        knockbackHandler = fsm.gameObject.GetComponent<KnockbackHandler>();
        specialMeter = fsm.gameObject.GetComponent<AdamSpecialMeter>();
        fsm.animator.PlayAnimation("DSpecial_Enter");
        EnableJumpCancel();
    }

    public void Exit()
    {
        DisableJumpCancel();
    }

    public void Update()
    {
        if (Mathf.Abs(fsm.input.MoveInput.x) > 0.01f)
        {
            //Debug.Log($"{walkHeld * Time.deltaTime} {walkTimeThreshold * Time.deltaTime}");
            walkHeld++;
        }
        else
        {
            walkHeld = 0;
        }
        if (fsm.input.Flick)
        {
            float flickX = fsm.input.FlickDirection.x;
            if (Mathf.Abs(flickX) > 0.8f)
            {
                fsm.SetDirection(flickX);
                fsm.input.ConsumeFlick();
                fsm.SetState(new InitialDashState());

                return;
            }
        }
        if (fsm.fip.Walk() && walkHeld >= 10)
        {
            fsm.SetDirection(fsm.input.MoveInput.x);
            fsm.SetState(new WalkState());
            return;
        }
        if (fsm.fip.IsHoldingJump())
        {
            //Debug.Log("helo");
            fsm.SetState(new JumpState());
        }
        if (fsm.input.Attack)
        {
            fsm.SetStateAttack(fsm.getAttack.GetAttack());
        }
        if (fsm.input.Strong)
        {
            fsm.SetStateAttack(fsm.getAttack.GetStrong());
        }
        if (fsm.input.Special)
        {
            fsm.SetState(fsm.specialManager.GetSpecial());
        }
        if (fsm.input.Shield)
        {
            fsm.SetState(new ShieldState());
        }
        if (fsm.input.Grab)
        {
            fsm.SetStateAttack(fsm.getAttack.GetGrab());
        }
    }

    public void FixedUpdate()
    {
        knockbackHandler.TakeDamage(damagePerFrame);
        specialMeter.AddCharge(chargePerFrame);
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
