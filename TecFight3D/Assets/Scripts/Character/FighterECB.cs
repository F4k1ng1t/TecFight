
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
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

    private Gravity gravity;

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
        gravity = GetComponent<Gravity>();
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
        RaycastHit hit;
        float resolvedX = displacement.x;
        if (displacement.x < 0)
        {
            if (Physics.Raycast(
                LeftPosition,
                Vector3.left,
                out hit,
                Mathf.Abs(displacement.x) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                resolvedX = 0;
                HitWall = true;
            }
            else if (Physics.Raycast(
                FeetPosition,
                Vector3.left,
                out hit,
                Mathf.Abs(displacement.y) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                Debug.Log("hit under: " + hit.collider.name);
                resolvedX = 0;
                GroundNormal = hit.normal;
            }
        }
        else if (displacement.x > 0)
        {
            if (Physics.Raycast(
                RightPosition,
                Vector3.right,
                out hit,
                Mathf.Abs(displacement.x) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                resolvedX = 0;
                HitWall = true;
            }
            else if (Physics.Raycast(
                FeetPosition,
                Vector3.right,
                out hit,
                Mathf.Abs(displacement.y) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                Debug.Log("hit under: " + hit.collider.name);
                resolvedX = 0;
                GroundNormal = hit.normal;
            }
        }

        float resolvedY = displacement.y;
        if (displacement.y < 0)
        {
            //raycast from feet down
            if (Physics.Raycast(
                FeetPosition,
                Vector3.down,
                out hit,
                Mathf.Abs(displacement.y) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                Debug.Log("hit under: " + hit.collider.name);
                resolvedY = 0;
                GroundNormal = hit.normal;
            }
            //if the side raycsasts hit the floor move the character outwards
            else if (Physics.Raycast(
                LeftPosition,
                Vector3.down,
                out hit,
                Mathf.Abs(displacement.x) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                Debug.Log("LeftUnder");
                resolvedX = halfWidth;
            }
            else if (Physics.Raycast(
                RightPosition,
                Vector3.down,
                out hit,
                Mathf.Abs(displacement.x) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                Debug.Log("RightUnder");
                resolvedX = -halfWidth;
            }
        }
        else if (displacement.y > 0)
        {
            if (Physics.Raycast(
                TopPosition,
                Vector3.up,
                out hit,
                Mathf.Abs(displacement.y) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                resolvedY = hit.distance;
                HitCeiling = true;
            }
        }

        Vector3 resolved = new Vector3(resolvedX, resolvedY, 0f);

        transform.position += resolved;
        LastResolvedPosition = transform.position;

        CheckGround();

        return resolved;
    }

    //this function is mostly fine, but we need some new behavior for moving down
    // on left or right, stop on any collision
    //on up/down, move outward on anything that isnt a foot/head collision
    private float ResolveAxis(Vector3 axis, float amount)
    {
        if (Mathf.Abs(amount) < 0.000001f)
            return 0f;

        Vector3 direction = axis * Mathf.Sign(amount);
        float allowedDistance = Mathf.Abs(amount);

        bool movingDown = axis == Vector3.up && amount < 0f;
        bool movingUp = axis == Vector3.up && amount > 0f;

        // One-way platforms are handled only by the feet during downward movement.
        if (enableOneWayPlatforms && movingDown)
        {
            float platformDistance = ResolveOneWayPlatformsDownward();

            if (platformDistance >= 0f)
                allowedDistance = Mathf.Min(allowedDistance, platformDistance);
        }

        foreach (Vector3 sample in GetBoundarySamples())
        {
            // The feet-only platform check above handles downward platform collisions.
            // Ignore platform colliders in all other ECB raycasts.
            Vector3 origin = sample - direction * skinWidth;

            //check the direction we want to move for collisions
            if (!Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                Mathf.Abs(amount) + skinWidth,
                collisionLayers,
                TriggerInteraction))
            {
                //if the raycast doesn't hit anything continue
                continue;
            }

            if (hit.collider.transform.IsChildOf(transform))
                continue;

            OneWayPlatform platform = enableOneWayPlatforms
                ? hit.collider.GetComponentInParent<OneWayPlatform>()
                : null;

            Debug.Log(gameObject.name + sample + " hit " + hit.collider.gameObject.name);
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
    /// Checks only the ECB's feet against one-way platforms while descending.
    /// Returns -1 if no platform is hit, otherwise the allowed downward distance.
    /// </summary>
    private float ResolveOneWayPlatformsDownward()
    {
        Vector3 feet = FeetPosition;
        Vector3 origin = feet + Vector3.up * skinWidth;

        float checkDistance = skinWidth + 1000f;
        float nearestDistance = float.MaxValue;
        bool foundPlatform = false;
        Vector3 nearestNormal = Vector3.up;

        RaycastHit[] hits = Physics.RaycastAll(
            origin,
            Vector3.down,
            checkDistance,
            collisionLayers,
            TriggerInteraction);

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

        if (enableOneWayPlatforms && gravity != null && gravity.verticalVelocity > 0f)
            return;

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

        if (platform != null && gravity != null && gravity.verticalVelocity > 0f)
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

        Vector3 left = feet + Vector3.up * (height * 0.5f) + Vector3.left * halfWidth;
        Vector3 right = feet + Vector3.up * (height * 0.5f) + Vector3.right * halfWidth;

        Vector3[] vertices = { feet, left, top, right };

        for (int edge = 0; edge < vertices.Length; edge++)
        {
            Vector3 a = vertices[edge];
            Vector3 b = vertices[(edge + 1) % vertices.Length];

            //the amount of point it checks between each part of the ECB Diamond
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

        Gizmos.DrawLine(FeetPosition, LeftPosition);
        Gizmos.DrawLine(LeftPosition, TopPosition);
        Gizmos.DrawLine(TopPosition, RightPosition);
        Gizmos.DrawLine(RightPosition, FeetPosition);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(feet + Vector3.up * skinWidth, feet + Vector3.down * groundCheckDistance);
    }
}
