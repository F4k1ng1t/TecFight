
using UnityEngine;

[RequireComponent(typeof(FighterStateMachine))]
[RequireComponent(typeof(FighterPhysics))]
public class GrabInteraction : MonoBehaviour
{
    [Header("Positioning")]
    public Transform holdPoint;
    public Vector3 holdOffset = new Vector3(0.8f, -1f, 0f);

    [Header("Animations")]
    public string grabAnimation = "Grab";
    public string grabbedAnimation = "GrabIdle";

    [Header("Throw")]
    public float forwardThrowSpeed = 12f;
    public float upwardThrowSpeed = 4f;

    private FighterStateMachine grabber;
    private FighterPhysics grabberPhysics;

    private FighterStateMachine victim;
    private FighterPhysics victimPhysics;

    private bool originalGrabberGravity;
    private bool originalVictimGravity;

    public bool IsGrabbing { get; private set; }

    private void Awake()
    {
        grabber = GetComponent<FighterStateMachine>();
        grabberPhysics = GetComponent<FighterPhysics>();
    }

    public bool StartGrab(FighterStateMachine target)
    {
        if (IsGrabbing || target == null || target == grabber)
            return false;

        if (target.GetComponent<GrabInteraction>()?.IsGrabbing == true)
            return false;

        victim = target;
        victimPhysics = victim.GetComponent<FighterPhysics>();

        if (victimPhysics == null)
            return false;

        IsGrabbing = true;

        // Determine which direction the victim should face.
        victim.SetDirection(-grabber.direction);

        // Save gravity settings.
        originalGrabberGravity = grabberPhysics.useGravity;
        originalVictimGravity = victimPhysics.useGravity;

        // Stop both fighters.
        grabberPhysics.ClearVelocity();
        victimPhysics.ClearVelocity();

        grabberPhysics.useGravity = false;
        victimPhysics.useGravity = false;

        // Position the victim at the hold point.
        Vector3 position = holdPoint != null ? holdPoint.position : grabber.transform.position + new Vector3(holdOffset.x * grabber.direction, holdOffset.y, holdOffset.z);

        victim.transform.position = position;

        // Enter grab states.
        grabber.SetState(new GrabState(this));
        victim.SetState(new GrabbedState());

        return true;
    }

    public void ReleaseThrow()
    {
        if (!IsGrabbing)
            return;

        Vector3 knockback = new Vector3(
            grabber.direction * forwardThrowSpeed,
            upwardThrowSpeed,
            0f
        );

        Release(knockback);
    }

    public void Release(Vector3 knockback)
    {
        if (!IsGrabbing)
            return;

        IsGrabbing = false;

        // Restore gravity.
        grabberPhysics.useGravity = originalGrabberGravity;
        victimPhysics.useGravity = originalVictimGravity;

        // Transition the fighters.
        grabber.SetState(new IdleState());
        victim.SetState(new HitState());

        // Apply throw knockback after entering HitState.
        victimPhysics.AddForce(knockback, ForceMode.VelocityChange);

        victim = null;
        victimPhysics = null;
    }

    public void CancelGrab()
    {
        if (!IsGrabbing)
            return;

        IsGrabbing = false;

        grabberPhysics.useGravity = originalGrabberGravity;
        victimPhysics.useGravity = originalVictimGravity;

        grabber.SetState(grabber.ecb.IsGrounded ? new IdleState() : new AirState());
        victim.SetState(victim.ecb.IsGrounded ? new IdleState() : new AirState());

        victim = null;
        victimPhysics = null;
    }

    public void Pummel(float damage)
    {
        if (!IsGrabbing || victim == null)
            return;

        KnockbackHandler knockbackHandler = victim.GetComponent<KnockbackHandler>();

        if (knockbackHandler != null)
        {
            knockbackHandler.OnHit(Vector3.zero, damage, Time.time, 0f);
        }
    }

    public void PlayGrabAnimation()
    {
        grabber.animator?.PlayAnimation(grabAnimation);
    }

    public void PlayGrabbedAnimation()
    {
        victim?.animator?.PlayAnimation(grabbedAnimation);
    }
}
