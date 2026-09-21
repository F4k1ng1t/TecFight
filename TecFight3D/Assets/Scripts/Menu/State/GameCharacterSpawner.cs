using UnityEngine;

public class GameCharacterSpawner : MonoBehaviour
{
    public CharacterData[] availableCharacters;
    public Transform spawnPoint;

    void Start()
    {
        int charIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        int attachIndex = PlayerPrefs.GetInt("SelectedAttachmentIndex", -1);

        CharacterData activeData = availableCharacters[charIndex];

     
        GameObject spawnedPlayer = Instantiate(activeData.characterPrefab, spawnPoint.position, Quaternion.identity);

        // Dynamically assign the ScriptableObject stats directly onto your gameplay runtime script
        if (attachIndex >= 0 && attachIndex < activeData.availableAttachments.Length)
        {
            AttachmentData activeAttachment = activeData.availableAttachments[attachIndex];
            // Instantiate weapon/item visual onto the runtime character model mesh structure here
            Debug.Log($"Spawned {activeData.characterName} with a {activeAttachment.attachmentName} attached!");
        }
    }
}
