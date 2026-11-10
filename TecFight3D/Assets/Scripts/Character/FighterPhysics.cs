
using UnityEngine;

[RequireComponent(typeof(FighterECB))]
[RequireComponent(typeof(FighterInput))]
public class FighterPhysics : MonoBehaviour
{
    [Header("Physics")]
    [Min(0.01f)] public float mass = 1f;
    [Min(0f)] public float gravity = 30f;
    public bool useGravity = true;

    [Header("Traction")]
    [Min(0f)] public float groundTraction = 60f;
    [Min(0f)] public float airFriction = 2f;
    [Min(0f)] public float stopThreshold = 0.05f;
    [Min(0f)] public float inputDeadzone = 0.2f;

    public Vector3 Velocity { get; private set; }

    // Results from the most recent physics simulation.
    public Vector3 LastResolvedMovement { get; private set; }
    public Vector3 LastRequestedMovement { get; private set; }
    public bool LastHitWall { get; private set; }
    public bool LastHitCeiling { get; private set; }
    public bool LastGrounded { get; private set; }

    private FighterECB ecb;
    private FighterInput input;

    private void Awake()
    {
        ecb = GetComponent<FighterECB>();
        input = GetComponent<FighterInput>();

    }

    public void Simulate()
    {
        float dt = Time.fixedDeltaTime;

        // Apply gravity.
        if (useGravity)
            Velocity += Vector3.down * gravity * dt;

        // Apply traction when there is no horizontal movement input.
        ApplyTraction(dt);

        // Move and resolve collisions.
        Vector3 displacement = Velocity * dt;

        LastRequestedMovement = displacement;
        LastResolvedMovement = ecb.Move(displacement);

        LastHitWall = ecb.HitWall;
        LastHitCeiling = ecb.HitCeiling;
        LastGrounded = ecb.IsGrounded;

        // Stop horizontal velocity when hitting a wall.
        if (LastHitWall)
            SetHorizontalVelocity(0f);

        // Stop upward velocity when hitting a ceiling.
        if (LastHitCeiling && Velocity.y > 0f)
            SetVerticalVelocity(0f);

        // Stop downward velocity when the floor blocks movement.
        if (displacement.y < 0f &&
            LastResolvedMovement.y > displacement.y + 0.0001f)
        {
            SetVerticalVelocity(0f);
        }

        if (LastGrounded && Velocity.y < 0f)
            SetVerticalVelocity(0f);
    }

    private void ApplyTraction(float dt)
    {
        bool hasMovementInput = Mathf.Abs(input.MoveInput.x) > inputDeadzone;

        if (hasMovementInput)
            return;

        float traction = ecb.IsGrounded ? groundTraction : airFriction;

        float newVelocity = Mathf.MoveTowards(
            Velocity.x,
            0f,
            traction * dt
        );

        if (Mathf.Abs(newVelocity) < stopThreshold)
            newVelocity = 0f;

        SetHorizontalVelocity(newVelocity);
    }

    public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        switch (mode)
        {
            case ForceMode.Force:
                Velocity += force / mass * Time.fixedDeltaTime;
                break;

            case ForceMode.Acceleration:
                Velocity += force * Time.fixedDeltaTime;
                break;

            case ForceMode.Impulse:
                Velocity += force / mass;
                break;

            case ForceMode.VelocityChange:
                Velocity += force;
                break;
        }

        Velocity = new Vector3(Velocity.x, Velocity.y, 0f);
    }

    public void SetVelocity(Vector3 velocity)
    {
        Velocity = new Vector3(velocity.x, velocity.y, 0f);
    }

    public void SetHorizontalVelocity(float velocity)
    {
        Velocity = new Vector3(velocity, Velocity.y, 0f);
    }

    public void SetVerticalVelocity(float velocity)
    {
        Velocity = new Vector3(Velocity.x, velocity, 0f);
    }

    public void ClearVelocity()
    {
        Velocity = Vector3.zero;
    }
}
