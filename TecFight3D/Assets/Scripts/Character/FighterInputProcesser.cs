using UnityEngine;

public class FighterInputProcesser : MonoBehaviour
{
    FighterStateMachine fsm;
    FighterInput fi;
    CharacterObject co;
    Rigidbody rig;
    public void Start()
    {
        fsm = GetComponent<FighterStateMachine>();
        fi = fsm.input;
        co = fsm.charObj;
        rig = fsm.rig;
    }
    public bool Walk()
    {
        return Mathf.Abs(fi.MoveInput.x) < 0.2f;
    }
    public bool Jump()
    {
        return fi.IsHoldingJump;
    }
    public bool Idle()
    {
        return Mathf.Abs(fi.MoveInput.x) < 0.05f;
    }
    public bool CompareCurrentAirDriftMax()
    {
        return Mathf.Abs(rig.linearVelocity.x) >= co.airSpeed && Mathf.Sign(fi.MoveInput.x) != Mathf.Sign(rig.linearVelocity.x) && fi.MoveInput.x != 0;
    }
    public bool FastFall()
    {
        return rig.linearVelocity.y <= 0 && fi.MoveInput.y < -0.5f;
    }
}
