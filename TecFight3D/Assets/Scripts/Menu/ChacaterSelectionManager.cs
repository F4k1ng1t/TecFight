using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    [Header("Character Configurations")]
    public CharacterData[] availableCharacters;

    [Header("Scene References")]
    public Transform characterSpawnPoint;
    public Transform attachmentSpawnPoint; // Where the weapon/item snaps onto the character

    [Header("UI Fields")]
    public TMPro.TMP_Text nameText;
    public TMPro.TMP_Text statsText; 

    [Header("Global UI Navigation Buttons")]
    public Button nextCharButton;
    public Button prevCharButton;
    public Button selectCharButton; 

    [Header("Vanilla Attachment UI Buttons")]
    public Button attachment1Button;
    public Button attachment2Button;
    public Button attachment3Button;

    public int currentCharacterIndex { get; set; } = 0;
    public int currentAttachmentIndex { get; set; } = -1; 
    private ISelectionState currentState;
    private GameObject spawnedCharacter;
    private GameObject spawnedAttachment;

    public BrowsingState BrowsingState = new BrowsingState();
    public LockedState LockedState = new LockedState();

    void Start()
    {
        SwitchState(BrowsingState);
    }

    void Update()
    {
        currentState?.UpdateState(this);
    }

    public void SwitchState(ISelectionState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void RefreshUIAndVisuals()
    {
        CharacterData data = availableCharacters[currentCharacterIndex];

        // 1. Update text fields and stats dynamically
        nameText.text = data.characterName;
        statsText.text = $"HP: {data.maxHealth}\nSPD: {data.moveSpeed}";

        // 2. Respawn character model
        if (spawnedCharacter != null) Destroy(spawnedCharacter);
        spawnedCharacter = Instantiate(data.characterPrefab, characterSpawnPoint.position, characterSpawnPoint.rotation);

        // 3. Respawn selected attachment visual if one is picked
        if (spawnedAttachment != null) Destroy(spawnedAttachment);
        if (currentAttachmentIndex >= 0 && currentAttachmentIndex < data.availableAttachments.Length)
        {
            AttachmentData attachData = data.availableAttachments[currentAttachmentIndex];
            if (attachData.attachmentPrefab != null)
            {
                // In a production game, you would search spawnedCharacter for a specific socket/bone transform
                spawnedAttachment = Instantiate(attachData.attachmentPrefab, attachmentSpawnPoint.position, attachmentSpawnPoint.rotation, spawnedCharacter.transform);
            }
        }
    }
}
