
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FighterStateMachine))]
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

    public Vector3 FeetPosition =>
        transform.TransformPoint(feetOffset);

    public Vector3 CenterPosition =>
        FeetPosition + Vector3.up * (height * 0.5f);

    public Vector3 TopPosition =>
        FeetPosition + Vector3.up * height;

    public Vector3 LeftPosition =>
        FeetPosition + Vector3.up * (height * 0.5f)
        + Vector3.left * halfWidth;

    public Vector3 RightPosition =>
        FeetPosition + Vector3.up * (height * 0.5f)
        + Vector3.right * halfWidth;

    private void Awake()
    {
        LastResolvedPosition = transform.position;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    /// <summary>
    /// Moves the fighter through the ECB collision system.
    /// Displacement is measured in world units.
    /// Z displacement is intentionally ignored.
    /// </summary>
    public Vector3 Move(Vector3 displacement)
    {
        HitWall = false;
        HitCeiling = false;

        float resolvedX = ResolveAxis(
            Vector3.right, displacement.x);

        float resolvedY = ResolveAxis(
            Vector3.up, displacement.y);

        Vector3 resolved = new Vector3(
            resolvedX,
            resolvedY,
            0f);

        transform.position += resolved;
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

        // Sample the boundary at the current position.
        foreach (Vector3 sample in GetBoundarySamples())
        {
            // Offset the origin slightly away from the ECB
            // to avoid starting the ray directly on a surface.
            Vector3 origin = sample - direction * skinWidth;

            if (!Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                Mathf.Abs(amount) + skinWidth,
                collisionLayers,
                collideWithTriggers
                    ? QueryTriggerInteraction.Collide
                    : QueryTriggerInteraction.Ignore))
            {
                continue;
            }

            // Ignore colliders belonging to this fighter.
            if (hit.collider.transform.IsChildOf(transform))
                continue;

            float safeDistance = Mathf.Max(
                0f, hit.distance - skinWidth);

            allowedDistance = Mathf.Min(
                allowedDistance, safeDistance);

            if (axis == Vector3.right)
                HitWall = true;

            if (axis == Vector3.up && amount < 0f &&
                hit.normal.y >= minimumGroundNormalY)
            {
                GroundNormal = hit.normal;
            }

            if (axis == Vector3.up && amount > 0f)
                HitCeiling = true;
        }

        return Mathf.Sign(amount) * allowedDistance;
    }

    private void CheckGround()
    {
        IsGrounded = false;
        GroundNormal = Vector3.up;

        // Probe from the bottom vertex.
        Vector3 origin = FeetPosition
            + Vector3.up * skinWidth;

        if (Physics.Raycast(
            origin,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance + skinWidth,
            collisionLayers,
            collideWithTriggers
                ? QueryTriggerInteraction.Collide
                : QueryTriggerInteraction.Ignore))
        {
            if (!hit.collider.transform.IsChildOf(transform) &&
                hit.normal.y >= minimumGroundNormalY)
            {
                IsGrounded = true;
                GroundNormal = hit.normal;
            }
        }
    }

    public void RefreshGrounded()
    {
        CheckGround();
    }

    /// <summary>
    /// Returns sample points along the four diamond edges.
    /// No depth or Z-axis sampling is performed.
    /// </summary>
    private Vector3[] GetBoundarySamples()
    {
        List<Vector3> samples = new List<Vector3>();

        Vector3 feet = FeetPosition;
        Vector3 top = feet + Vector3.up * height;

        Vector3 left = feet
            + Vector3.up * (height * 0.5f)
            + Vector3.left * halfWidth;

        Vector3 right = feet
            + Vector3.up * (height * 0.5f)
            + Vector3.right * halfWidth;

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

        Vector3 left = feet
            + Vector3.up * (height * 0.5f)
            + Vector3.left * halfWidth;

        Vector3 right = feet
            + Vector3.up * (height * 0.5f)
            + Vector3.right * halfWidth;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(feet, left);
        Gizmos.DrawLine(left, top);
        Gizmos.DrawLine(top, right);
        Gizmos.DrawLine(right, feet);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(
            feet + Vector3.up * skinWidth,
            feet + Vector3.down * groundCheckDistance);
    }
}
