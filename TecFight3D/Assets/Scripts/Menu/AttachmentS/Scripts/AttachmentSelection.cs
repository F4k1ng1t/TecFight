using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttachmentSelection : MonoBehaviour
{
    public AttachmentSelectionInfo[] attachmentModels;
    public Transform Spot;
    public TextMeshProUGUI title;
    public TextMeshProUGUI infomation;

    [SerializeField] int playerNum = 1;
    private List<GameObject> Attachment;
    private int currentAttachment;


    private void Awake()
    {
        title = title.GetComponent<TextMeshProUGUI>();
        infomation = infomation.GetComponent<TextMeshProUGUI>();

    }
    void Start()
    {
        Attachment = new List<GameObject>();

        foreach (var attachmentModel in attachmentModels)
        {
            GameObject go  = Instantiate(attachmentModel.Attachment,
                Spot.position, Quaternion.Euler(new Vector3(0,0,90)));
           go.SetActive(false);
           go.transform.SetParent(Spot); 
           Attachment.Add(go);
        }

        ShowAttachmentList();
    }

    void ShowAttachmentList()
    {
        Attachment[currentAttachment].SetActive(true);
        title.text = attachmentModels[currentAttachment].Name;
        infomation.text = attachmentModels[currentAttachment].Info;
    }

    public void OnClickNext()
    {
        Attachment[currentAttachment].SetActive(false);

        if (currentAttachment < Attachment.Count - 1)
            currentAttachment = currentAttachment + 1;
        else
            currentAttachment = 0;
        UpdateTransferFile();

        ShowAttachmentList();
    }

    public void OnClickPrev()
    {
        Attachment[currentAttachment].SetActive(false);

        if (currentAttachment ==0) 
            currentAttachment = Attachment.Count -1;
        else
            currentAttachment = currentAttachment -1;
        UpdateTransferFile();

        ShowAttachmentList();
    }

    private void UpdateTransferFile()
    {
        switch (playerNum)
        {
            case 1:
                AttachmentTransfer.p1Attachment = currentAttachment;
                break;
            case 2:
                AttachmentTransfer.p2Attachment = currentAttachment;
                break;
        }
    }
    
}
