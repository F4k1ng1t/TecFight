
using UnityEngine;

public class FighterInputProcesser : MonoBehaviour
{
    private FighterStateMachine fsm;
    private FighterInput fi;
    private CharacterObject co;
    private Gravity gravity;

    private bool prevJumpInput;

    private void Awake()
    {
        fsm = GetComponent<FighterStateMachine>();
        fi = GetComponent<FighterInput>();
        gravity = GetComponent<Gravity>();
    }

    private void Start()
    {
        co = fsm.charObj;
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
        return !prevJumpInput && fi.IsHoldingJump;
    }

    public bool Idle()
    {
        return Mathf.Abs(fi.MoveInput.x) < 0.05f;
    }

    public bool CompareCurrentAirSpeedMax()
    {
        return Mathf.Abs(gravity.horizontalVelocity) >= co.airSpeed;
    }

    public bool AttemptMoveOpposite()
    {
        float inputX = fi.MoveInput.x;
        float velocityX = gravity.horizontalVelocity;

        return Mathf.Abs(inputX) > 0.2f &&
               Mathf.Abs(velocityX) > 0.01f &&
               Mathf.Sign(inputX) != Mathf.Sign(velocityX);
    }

    public bool FastFall()
    {
        if (gravity == null || fi == null)
            return false;

        return gravity.verticalVelocity <= 0f &&
               fi.MoveInput.y < -0.5f;
    }

    private void FixedUpdate()
    {
        prevJumpInput = fi.IsHoldingJump;
    }
}
