
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FighterStateMachine))]
[RequireComponent(typeof(FighterPhysics))]
public class FighterECB : MonoBehaviour
{
    [Header("Diamond Dimensions")]
    [Min(0.01f)] public float halfWidth = 0.4f;
    [Min(0.01f)] public float height = 2f;

    [Header("Position")]
    [Tooltip("Local position of the bottom vertex relative to this transform.")]
    public Vector3 feetOffset = Vector3.zero;

    [Header("Collision")]
    public LayerMask collisionLayers = ~0;
    [Min(0.001f)] public float skinWidth = 0.02f;
    [Range(1, 8)] public int edgeSamples = 3;
    public bool collideWithTriggers = false;

    [Header("One-Way Platforms")]
    public bool enableOneWayPlatforms = true;

    [Header("Grounding")]
    [Min(0.001f)] public float groundCheckDistance = 0.08f;
    [Range(0f, 1f)] public float minimumGroundNormalY = 0.55f;

    [Header("Debug")]
    public bool drawGizmos = true;

    public bool IsGrounded { get; private set; }
    public bool HitWall { get; private set; }
    public bool HitCeiling { get; private set; }

    public Vector3 GroundNormal { get; private set; } = Vector3.up;
    public Vector3 LastResolvedPosition { get; private set; }

    private FighterPhysics physics;

    private QueryTriggerInteraction TriggerInteraction =>
        collideWithTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

    public Vector3 FeetPosition => transform.TransformPoint(feetOffset);
    public Vector3 CenterPosition => FeetPosition + Vector3.up * (height * 0.5f);
    public Vector3 TopPosition => FeetPosition + Vector3.up * height;

    public Vector3 LeftPosition =>
        FeetPosition + Vector3.up * (height * 0.5f) + Vector3.left * halfWidth;

    public Vector3 RightPosition =>
        FeetPosition + Vector3.up * (height * 0.5f) + Vector3.right * halfWidth;

    private void Awake()
    {
        physics = GetComponent<FighterPhysics>();
        LastResolvedPosition = transform.position;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    public Vector3 Move(Vector3 displacement)
    {
        HitWall = false;
        HitCeiling = false;
        GroundNormal = Vector3.up;

        float resolvedX = ResolveAxis(Vector3.right, displacement.x);
        transform.position += Vector3.right * resolvedX;

        float resolvedY = ResolveAxis(Vector3.up, displacement.y);
        transform.position += Vector3.up * resolvedY;

        Vector3 resolved = new Vector3(resolvedX, resolvedY, 0f);

        LastResolvedPosition = transform.position;

        CheckGround();

        return resolved;
    }

    private float ResolveAxis(Vector3 axis, float amount)
    {
        if (Mathf.Abs(amount) < 0.000001f)
            return 0f;

        Vector3 direction = axis * Mathf.Sign(amount);
        float allowedDistance = Mathf.Abs(amount);

        bool movingDown = axis == Vector3.up && amount < 0f;
        bool movingUp = axis == Vector3.up && amount > 0f;

        // One-way platforms are checked only at the feet while descending.
        if (enableOneWayPlatforms && movingDown)
        {
            float platformDistance = ResolveOneWayPlatformsDownward(Mathf.Abs(amount));

            if (platformDistance >= 0f)
                allowedDistance = Mathf.Min(allowedDistance, platformDistance);
        }

        foreach (Vector3 sample in GetBoundarySamples())
        {
            Vector3 origin = sample - direction * skinWidth;

            if (!Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                Mathf.Abs(amount) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                continue;
            }

            if (hit.collider.transform.IsChildOf(transform))
                continue;

            OneWayPlatform platform = enableOneWayPlatforms
                ? hit.collider.GetComponentInParent<OneWayPlatform>()
                : null;

            // One-way platforms are handled separately.
            if (platform != null)
                continue;

            float safeDistance = Mathf.Max(0f, hit.distance - skinWidth);
            allowedDistance = Mathf.Min(allowedDistance, safeDistance);

            if (axis == Vector3.right)
                HitWall = true;

            if (movingDown && hit.normal.y >= minimumGroundNormalY)
                GroundNormal = hit.normal;

            if (movingUp)
                HitCeiling = true;
        }

        return Mathf.Sign(amount) * allowedDistance;
    }

    /// <summary>
    /// Checks only the feet against one-way platforms while descending.
    /// Returns -1 if no platform is hit.
    /// Otherwise returns the allowed downward distance.
    /// </summary>
    private float ResolveOneWayPlatformsDownward(float movementDistance)
    {
        Vector3 origin = FeetPosition + Vector3.up * skinWidth;

        float checkDistance = movementDistance + skinWidth;
        float nearestDistance = float.MaxValue;

        bool foundPlatform = false;
        Vector3 nearestNormal = Vector3.up;

        RaycastHit[] hits = Physics.RaycastAll(
            origin,
            Vector3.down,
            checkDistance,
            collisionLayers,
            TriggerInteraction
        );

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform))
                continue;

            OneWayPlatform platform = hit.collider.GetComponentInParent<OneWayPlatform>();

            if (platform == null)
                continue;

            if (hit.normal.y < minimumGroundNormalY)
                continue;

            if (hit.distance < nearestDistance)
            {
                nearestDistance = hit.distance;
                nearestNormal = hit.normal;
                foundPlatform = true;
            }
        }

        if (!foundPlatform)
            return -1f;

        GroundNormal = nearestNormal;

        return Mathf.Max(0f, nearestDistance - skinWidth);
    }

    private void CheckGround()
    {
        IsGrounded = false;
        GroundNormal = Vector3.up;

        Vector3 origin = FeetPosition + Vector3.up * skinWidth;

        if (!Physics.Raycast(
            origin,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance + skinWidth,
            collisionLayers,
            TriggerInteraction))
        {
            return;
        }

        if (hit.collider.transform.IsChildOf(transform))
            return;

        OneWayPlatform platform = enableOneWayPlatforms
            ? hit.collider.GetComponentInParent<OneWayPlatform>()
            : null;

        // Only ignore one-way platforms while rising.
        if (platform != null && physics != null && physics.Velocity.y > 0f)
            return;

        if (hit.normal.y >= minimumGroundNormalY)
        {
            IsGrounded = true;
            GroundNormal = hit.normal;
        }
    }

    public void RefreshGrounded()
    {
        CheckGround();
    }

    private Vector3[] GetBoundarySamples()
    {
        List<Vector3> samples = new List<Vector3>();

        Vector3 feet = FeetPosition;
        Vector3 top = feet + Vector3.up * height;

        Vector3 left = FeetPosition + Vector3.up * (height * 0.5f) + Vector3.left * halfWidth;
        Vector3 right = FeetPosition + Vector3.up * (height * 0.5f) + Vector3.right * halfWidth;

        Vector3[] vertices = { feet, left, top, right };

        for (int edge = 0; edge < vertices.Length; edge++)
        {
            Vector3 a = vertices[edge];
            Vector3 b = vertices[(edge + 1) % vertices.Length];

            for (int i = 0; i <= edgeSamples; i++)
            {
                float t = (float)i / edgeSamples;
                samples.Add(Vector3.Lerp(a, b, t));
            }
        }

        return samples.ToArray();
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos)
            return;

        Vector3 feet = transform.TransformPoint(feetOffset);
        Vector3 top = feet + Vector3.up * height;

        Vector3 left = feet + Vector3.up * (height * 0.5f) + Vector3.left * halfWidth;
        Vector3 right = feet + Vector3.up * (height * 0.5f) + Vector3.right * halfWidth;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(feet, left);
        Gizmos.DrawLine(left, top);
        Gizmos.DrawLine(top, right);
        Gizmos.DrawLine(right, feet);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            feet + Vector3.up * skinWidth,
            feet + Vector3.down * groundCheckDistance
        );
    }
}
