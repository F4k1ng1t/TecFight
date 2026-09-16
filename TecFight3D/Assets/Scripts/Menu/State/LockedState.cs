using UnityEngine;
using UnityEngine.SceneManagement;

public class LockedState : ISelectionState
{
    public void EnterState(CharacterSelectionManager manager)
    {
        // Save indices to PlayerPrefs (or pass directly to an active persistent GameManager instance)
        PlayerPrefs.SetInt("SelectedCharacterIndex", manager.currentCharacterIndex);
        PlayerPrefs.SetInt("SelectedAttachmentIndex", manager.currentAttachmentIndex);
        PlayerPrefs.Save();

        Debug.Log("Character configuration finalized and saved.");

        // Load gameplay scene
        SceneManager.LoadScene("GameplayScene");
    }

    public void UpdateState(CharacterSelectionManager manager) { }
    public void ExitState(CharacterSelectionManager manager) { }
}
