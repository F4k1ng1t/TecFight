using UnityEngine;

[CreateAssetMenu(fileName = "Hitbox", menuName = "Scriptable Objects/Hitbox")]
public class Hitbox : ScriptableObject
{

    public int bindingIndex;
    public Vector3 offset;
    public int activeFrames = 1;
    public float radius = 0.005f;
    public float height = 0;
    [Range(0, 360)] public float sendAngle = 90;
    public float knockback = 0.1f;
    public float damage = 1f;
    public int priority = 0;
}
