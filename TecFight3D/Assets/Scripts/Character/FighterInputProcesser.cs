using UnityEngine;

public class FighterInputProcesser : MonoBehaviour
{
    [HideInInspector] public FighterStateMachine fsm;
    FighterInput fi;
    public void Start()
    {
        fsm = GetComponent<FighterStateMachine>();
        fi = fsm.input;
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
}
