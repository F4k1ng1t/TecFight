
using UnityEngine;

[RequireComponent(typeof(FighterECB))]
[RequireComponent(typeof(FighterStateMachine))]
public class FighterGrounded : MonoBehaviour
{
    public bool isGrounded = false;

    private bool wasGrounded = false;

    private FighterStateMachine fsm;
    private FighterECB ecb;

    private void Awake()
    {
        fsm = GetComponent<FighterStateMachine>();
        ecb = GetComponent<FighterECB>();
    }

    private void Start()
    {
        ecb.RefreshGrounded();

        isGrounded = ecb.IsGrounded;
        wasGrounded = isGrounded;

        if (isGrounded)
            fsm.SetState(new GroundedState());
        else
            fsm.SetState(new AirState());
    }

    private void FixedUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = ecb.IsGrounded;

        if (!isGrounded && wasGrounded)
        {
            fsm.SetState(new AirState());
        }
        else if (isGrounded && !wasGrounded)
        {
            fsm.SetState(new GroundedState());
        }
    }
}
