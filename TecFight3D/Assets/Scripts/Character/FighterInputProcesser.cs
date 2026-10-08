using UnityEngine;

public class FighterInputProcesser : MonoBehaviour
{
    FighterStateMachine fsm;
    FighterInput fi;
    CharacterObject co;
    Rigidbody rig;

    private bool prevJumpInput = false;

    public void Start()
    {
        fsm = GetComponent<FighterStateMachine>();
        fi = fsm.input;
        co = fsm.charObj;
        rig = fsm.rig;
    }

    public bool Walk()
    {
        return Mathf.Abs(fi.MoveInput.x) > 0.2f;
    }

    public bool IsHoldingJump()
    {
        return fi.IsHoldingJump;
    }
    public bool JumpPressed()
    {
        if (!prevJumpInput)
        {
            return fi.IsHoldingJump;
        }
        return false;
    }
    public bool Idle()
    {
        return Mathf.Abs(fi.MoveInput.x) < 0.05f;
    }

    public bool CompareCurrentAirSpeedMax()
    {
        return rig.linearVelocity.x > co.airSpeed;
    }
    public bool AttemptMoveOpposite()
    {
        return Mathf.Sign(fi.MoveInput.x) != Mathf.Sign(rig.linearVelocity.x);
    }
    public bool FastFall()
    {
        return rig.linearVelocity.y <= 0 &&
               fi.MoveInput.y < -0.5f;
    }
    private void FixedUpdate()
    {
        prevJumpInput = fi.IsHoldingJump;
    }
}