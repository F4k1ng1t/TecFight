using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character Selection/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public GameObject characterPrefab;

    [Header("Modular Stats")]
    public float maxHealth;
    public float moveSpeed;

    [Header("Available Attachments (Choose Up to 3)")]
    public AttachmentData[] availableAttachments;
}
