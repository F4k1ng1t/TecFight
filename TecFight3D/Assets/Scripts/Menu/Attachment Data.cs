using UnityEngine;

[CreateAssetMenu(fileName = "NewAttachment", menuName = "Character Selection/Attachment")]
public class AttachmentData : ScriptableObject
{
    public string attachmentName;
    public GameObject attachmentPrefab;
    // Add attachment stats here later (public int bonusDamage;)
}
