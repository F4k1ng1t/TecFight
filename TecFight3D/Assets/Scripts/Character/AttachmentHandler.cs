using UnityEngine;

public class AttachmentHandler : MonoBehaviour
{
    [SerializeField] GameObject[] attachments;
    [SerializeField] Transform attachPoint;

    public void SetAttachment(int attachmentIndex)
    {
        GameObject a = Instantiate(attachments[attachmentIndex], attachPoint);
    }
}
