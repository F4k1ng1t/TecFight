using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    [Header("Character Database")]
    public CharacterData[] characters;

    [System.Serializable]
    public class PlayerSelectionSlot
    {
        public int playerNumber;
        public TextMeshProUGUI nameText;
        public Transform previewSpawnPoint;
        public Transform gridParent;

        [HideInInspector] public int selectedIndex = 0;
        [HideInInspector] public GameObject currentPreviewInstance;
        [HideInInspector] public float inputCooldown = 0f;
    }

    [Header("Player Configurations")]
    public PlayerSelectionSlot player1Slot;
    public PlayerSelectionSlot player2Slot;

    [Header("UI Templates")]
    public GameObject gridButtonPrefab; 

    [Header("Input Axis Names")]
    public string p1HorizontalAxis = "Horizontal";
    public string p2HorizontalAxis = "HorizontalTurn";
    public string confirmButton = "Submit";

    private const string P1_SaveKey = "P1_SelectedCharacter";
    private const string P2_SaveKey = "P2_SelectedCharacter";

    void Start()
    {
        GenerateSelectionGrid(player1Slot);
        GenerateSelectionGrid(player2Slot);

        UpdatePlayerSelection(player1Slot, 0);
        UpdatePlayerSelection(player2Slot, 1);
    }

    private void GenerateSelectionGrid(PlayerSelectionSlot slot)
    {
        if (slot.gridParent == null || gridButtonPrefab == null) return;

        foreach (Transform child in slot.gridParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < characters.Length; i++)
        {
            int index = i; 
            GameObject newButton = Instantiate(gridButtonPrefab, slot.gridParent);

            Image buttonImage = newButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = characters[index].characterIcon;
            }

            Button btnComponent = newButton.GetComponent<Button>();
            if (btnComponent != null)
            {
                btnComponent.onClick.AddListener(() => SelectCharacter(slot.playerNumber, index));
            }
        }
    }


    void Update()
    {
        HandlePlayerInput(player1Slot, p1HorizontalAxis);
        HandlePlayerInput(player2Slot, p2HorizontalAxis);

        if (Input.GetButtonDown(confirmButton))
        {
            ConfirmAndLoadScene("GameplayScene");
        }
    }

    public void SelectCharacter(int playerNum, int characterIndex)
    {
        if (characterIndex < 0 || characterIndex >= characters.Length) return;
        PlayerSelectionSlot slot = (playerNum == 1) ? player1Slot : player2Slot;
        UpdatePlayerSelection(slot, characterIndex);
    }

    private void HandlePlayerInput(PlayerSelectionSlot slot, string axisName)
    {
        if (slot.inputCooldown > 0)
        {
            slot.inputCooldown -= Time.deltaTime;
            return;
        }

        float input = Input.GetAxisRaw(axisName);

        if (Mathf.Abs(input) > 0.5f)
        {
            int newIndex = slot.selectedIndex;

            if (input > 0) newIndex++;
            else if (input < 0) newIndex--;

            if (newIndex >= characters.Length) newIndex = 0;
            if (newIndex < 0) newIndex = characters.Length - 1;

            UpdatePlayerSelection(slot, newIndex);
            slot.inputCooldown = 0.25f;
        }
    }

    private void UpdatePlayerSelection(PlayerSelectionSlot slot, int index)
    {
        slot.selectedIndex = index;
        CharacterData activeData = characters[index];

        if (slot.nameText != null) slot.nameText.text = activeData.characterName;

        if (slot.currentPreviewInstance != null)
        {
            Destroy(slot.currentPreviewInstance);
        }

        if (activeData.previewModelPrefab != null && slot.previewSpawnPoint != null)
        {
            slot.currentPreviewInstance = Instantiate(
                activeData.previewModelPrefab,
                slot.previewSpawnPoint.position,
                slot.previewSpawnPoint.rotation
            );

            Camera stageCamera = slot.previewSpawnPoint.parent.GetComponentInChildren<Camera>();

            if (stageCamera != null)
            {
                Vector3 directionToCamera = stageCamera.transform.position - slot.currentPreviewInstance.transform.position;
                directionToCamera.y = 0;

                if (directionToCamera != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);

                    Quaternion offsetRotation = Quaternion.Euler(0, activeData.rotationOffset, 0);

                    slot.currentPreviewInstance.transform.rotation = lookRotation * offsetRotation;
                }
            }
        }


    }


    public void ConfirmAndLoadScene(string gameplaySceneName)
    {
        PlayerPrefs.SetInt(P1_SaveKey, player1Slot.selectedIndex);
        PlayerPrefs.SetInt(P2_SaveKey, player2Slot.selectedIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameplaySceneName);
    }
}
