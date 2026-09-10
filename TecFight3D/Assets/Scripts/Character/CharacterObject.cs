using UnityEngine;

[CreateAssetMenu(fileName = "CharacterObject", menuName = "Scriptable Objects/CharacterObject")]
public class CharacterObject : ScriptableObject
{
    public float airAccel;
    public float airSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float weight;
    public float fallSpeed;
    public float fallAccel;
    public float fastfallSpeed;
}
