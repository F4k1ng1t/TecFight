using UnityEngine;

public class BrowsingState : ISelectionState
{
    public void EnterState(CharacterSelectionManager manager)
    {
        manager.currentAttachmentIndex = -1; // Reset attachment choice on entry
        manager.RefreshUIAndVisuals();

        // Bind cycle buttons
        manager.nextCharButton.onClick.AddListener(() => CycleCharacter(manager, 1));
        manager.prevCharButton.onClick.AddListener(() => CycleCharacter(manager, -1));

        // Vanilla logic: Bind the 3 explicit attachment options
        manager.attachment1Button.onClick.AddListener(() => SelectAttachment(manager, 0));
        manager.attachment2Button.onClick.AddListener(() => SelectAttachment(manager, 1));
        manager.attachment3Button.onClick.AddListener(() => SelectAttachment(manager, 2));

        // Player must press this button explicitly to choose/lock-in the character
        manager.selectCharButton.onClick.AddListener(() => manager.SwitchState(manager.LockedState));
    }

    public void UpdateState(CharacterSelectionManager manager) { }

    public void ExitState(CharacterSelectionManager manager)
    {
        // Unbind everything safely to prevent memory leaks
        manager.nextCharButton.onClick.RemoveAllListeners();
        manager.prevCharButton.onClick.RemoveAllListeners();
        manager.attachment1Button.onClick.RemoveAllListeners();
        manager.attachment2Button.onClick.RemoveAllListeners();
        manager.attachment3Button.onClick.RemoveAllListeners();
        manager.selectCharButton.onClick.RemoveAllListeners();
    }

    private void CycleCharacter(CharacterSelectionManager manager, int direction)
    {
        manager.currentCharacterIndex += direction;
        if (manager.currentCharacterIndex >= manager.availableCharacters.Length) manager.currentCharacterIndex = 0;
        if (manager.currentCharacterIndex < 0) manager.currentCharacterIndex = manager.availableCharacters.Length - 1;

        manager.currentAttachmentIndex = -1; // Clear attachment when viewing someone else
        manager.RefreshUIAndVisuals();
    }

    private void SelectAttachment(CharacterSelectionManager manager, int attachmentSlotIndex)
    {
        CharacterData currentChar = manager.availableCharacters[manager.currentCharacterIndex];

        // Ensure the character data actually has an attachment configured in that specific slot
        if (currentChar.availableAttachments != null && attachmentSlotIndex < currentChar.availableAttachments.Length)
        {
            manager.currentAttachmentIndex = attachmentSlotIndex;
            manager.RefreshUIAndVisuals();
            Debug.Log($"Selected Attachment: {currentChar.availableAttachments[attachmentSlotIndex].attachmentName}");
        }
    }
}
