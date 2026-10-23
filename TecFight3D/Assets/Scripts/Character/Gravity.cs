
using UnityEngine;

[RequireComponent(typeof(FighterStateMachine))]
[RequireComponent(typeof(FighterECB))]
public class Gravity : MonoBehaviour
{
    private FighterStateMachine fsm;
    private FighterECB ecb;

    [Header("Gravity")]
    public bool active = true;
    public float verticalVelocity;

    [Header("Horizontal Movement")]
    public float horizontalVelocity;
    public bool clearHorizontalVelocityOnLanding = true;

    private void Awake()
    {
        fsm = GetComponent<FighterStateMachine>();
        ecb = GetComponent<FighterECB>();
    }

    private void FixedUpdate()
    {
        if (!active || fsm.charObj == null)
            return;

        verticalVelocity -= fsm.charObj.fallAccel * Time.fixedDeltaTime;

        float horizontalDisplacement = 0f;

        if (!ecb.IsGrounded)
            horizontalDisplacement = horizontalVelocity * Time.fixedDeltaTime;

        Vector3 displacement = new Vector3(
            horizontalDisplacement,
            verticalVelocity * Time.fixedDeltaTime,
            0f
        );

        Vector3 resolved = ecb.Move(displacement);

        if (Mathf.Abs(resolved.y - displacement.y) > 0.001f)
            verticalVelocity = 0f;

        if (ecb.IsGrounded && verticalVelocity <= 0f)
        {
            verticalVelocity = 0f;

            if (clearHorizontalVelocityOnLanding)
                horizontalVelocity = 0f;
        }
    }

    public void ResetVerticalVelocity()
    {
        verticalVelocity = 0f;
    }

    public void ResetHorizontalVelocity()
    {
        horizontalVelocity = 0f;
    }

    public void ResetVelocity()
    {
        verticalVelocity = 0f;
        horizontalVelocity = 0f;
    }
}
