
using UnityEngine;

public class FighterInputProcesser : MonoBehaviour
{
    private FighterStateMachine fsm;
    private FighterInput fi;
    private CharacterObject co;
    private FighterPhysics physics;

    private bool prevJumpInput;

    private void Awake()
    {
        fsm = GetComponent<FighterStateMachine>();
        fi = GetComponent<FighterInput>();
        physics = GetComponent<FighterPhysics>();
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
        return physics != null &&
               Mathf.Abs(physics.Velocity.x) >= co.airSpeed;
    }

    public bool AttemptMoveOpposite()
    {
        if (physics == null || fi == null)
            return false;

        float inputX = fi.MoveInput.x;
        float velocityX = physics.Velocity.x;

        return Mathf.Abs(inputX) > 0.2f &&
               Mathf.Abs(velocityX) > 0.01f &&
               Mathf.Sign(inputX) != Mathf.Sign(velocityX);
    }

    public bool FastFall()
    {
        if (physics == null || fi == null)
            return false;

        return physics.Velocity.y <= 0f &&
               fi.MoveInput.y < -0.5f;
    }

    private void FixedUpdate()
    {
        prevJumpInput = fi.IsHoldingJump;
    }
}
