using UnityEngine;

[CreateAssetMenu(fileName = "AttachmentSelectionInfo", menuName = "AttachmentSelectionInfo")]
public class AttachmentSelectionInfo : ScriptableObject
{
    public string Name;

    public string Info;

    public GameObject Attachment; 
}
