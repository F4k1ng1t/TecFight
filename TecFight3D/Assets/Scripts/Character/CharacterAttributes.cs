using UnityEngine;
[CreateAssetMenu(fileName = "CharacterAttributes", menuName = "ScriptableObjects/Character Attributes")]
public class CharacterAttributes : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public float airSpeed;
    public float weight;
    public float traction;
}
