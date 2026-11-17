using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttachmentSelection : MonoBehaviour
{
    public AttachmentSelectionInfo[] attachmentModels;
    public Transform Spot;
    public TextMeshProUGUI title;
    public TextMeshProUGUI infomation;

    [Header("UI Safe Zone Settings")]
    public RectTransform safeZoneUI;
    public float distanceFromCamera = 5f;
    [Range(0.1f, 1f)]
    public float fillPadding = 0.8f;

    [SerializeField] int playerNum = 1;
    private List<GameObject> Attachment;
    private int currentAttachment;

    private void Awake()
    {
        if (title != null) title = title.GetComponent<TextMeshProUGUI>();
        if (infomation != null) infomation = infomation.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        PositionSpotInSafeZone();

        Attachment = new List<GameObject>();

        foreach (var attachmentModel in attachmentModels)
        {
            GameObject go = Instantiate(
                attachmentModel.Attachment,
                Spot.position,
                Quaternion.Euler(new Vector3(0, 0, 90))
            );
            go.transform.SetParent(Spot);

            FitObjectToSafeZone(go);

            go.SetActive(false);
            Attachment.Add(go);
        }

        ShowAttachmentList();
    }


    private void PositionSpotInSafeZone()
    {
        if (safeZoneUI == null || Camera.main == null) return;

        Vector3[] corners = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        safeZoneUI.GetWorldCorners(corners);

        Vector3 bottomLeft = (Vector3)corners.GetValue(0);
        Vector3 topRight = (Vector3)corners.GetValue(2);

        Vector2 screenCenter = (bottomLeft + topRight) * 0.5f;
        Vector3 screenPointWithDepth = new Vector3(screenCenter.x, screenCenter.y, distanceFromCamera);
        Spot.position = Camera.main.ScreenToWorldPoint(screenPointWithDepth);
    }

    private void FitObjectToSafeZone(GameObject obj)
    {
        if (safeZoneUI == null || Camera.main == null) return;

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds combinedBounds = ((Renderer)renderers.GetValue(0)).bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            Renderer r = (Renderer)renderers.GetValue(i);
            combinedBounds.Encapsulate(r.bounds);
        }

        Vector3 centerOffset = combinedBounds.center - Spot.position;
        obj.transform.position -= centerOffset;

        combinedBounds = ((Renderer)renderers.GetValue(0)).bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(((Renderer)renderers.GetValue(i)).bounds);
        }

        Vector3 min = combinedBounds.min;
        Vector3 max = combinedBounds.max;

        Vector3[] boxCorners = new Vector3[]
        {
            new Vector3(min.x, min.y, min.z),
            new Vector3(min.x, min.y, max.z),
            new Vector3(min.x, max.y, min.z),
            new Vector3(min.x, max.y, max.z),
            new Vector3(max.x, min.y, min.z),
            new Vector3(max.x, min.y, max.z),
            new Vector3(max.x, max.y, min.z),
            new Vector3(max.x, max.y, max.z)
        };

        float minScreenX = float.MaxValue;
        float maxScreenX = float.MinValue;
        float minScreenY = float.MaxValue;
        float maxScreenY = float.MinValue;

        foreach (Vector3 worldCorner in boxCorners)
        {
            Vector3 screenPt = Camera.main.WorldToScreenPoint(worldCorner);
            if (screenPt.x < minScreenX) minScreenX = screenPt.x;
            if (screenPt.x > maxScreenX) maxScreenX = screenPt.x;
            if (screenPt.y < minScreenY) minScreenY = screenPt.y;
            if (screenPt.y > maxScreenY) maxScreenY = screenPt.y;
        }

        float objScreenWidth = maxScreenX - minScreenX;
        float objScreenHeight = maxScreenY - minScreenY;

        Vector3[] uiCorners = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        safeZoneUI.GetWorldCorners(uiCorners);
        Vector3 uiBottomLeft = (Vector3)uiCorners.GetValue(0);
        Vector3 uiTopRight = (Vector3)uiCorners.GetValue(2);

        float safeScreenWidth = Mathf.Abs(uiTopRight.x - uiBottomLeft.x) * fillPadding;
        float safeScreenHeight = Mathf.Abs(uiTopRight.y - uiBottomLeft.y) * fillPadding;

        if (objScreenWidth > 0.01f && objScreenHeight > 0.01f)
        {
            float scaleRatioX = safeScreenWidth / objScreenWidth;
            float scaleRatioY = safeScreenHeight / objScreenHeight;
            float finalScale = Mathf.Min(scaleRatioX, scaleRatioY);

            obj.transform.localScale *= finalScale;

            Bounds finalBounds = ((Renderer)renderers.GetValue(0)).bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                finalBounds.Encapsulate(((Renderer)renderers.GetValue(i)).bounds);
            }
            obj.transform.position -= (finalBounds.center - Spot.position);
        }
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

        if (currentAttachment == 0)
            currentAttachment = Attachment.Count - 1;
        else
            currentAttachment = currentAttachment - 1;

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
