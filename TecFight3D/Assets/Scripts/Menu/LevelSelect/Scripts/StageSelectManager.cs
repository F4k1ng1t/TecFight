using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    public Button[] stageButtons;

    void Start()
    {
        int levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);

        for (int i = 0; i < stageButtons.Length; i++)
        {
            if (i + 1 > levelsUnlocked)
            {
                stageButtons[i].interactable = false;
            }
            else
            {
                stageButtons[i].interactable = true;
            }
        }
    }

    public void LoadStage(string stageName)
    {
        SceneManager.LoadScene(stageName);
    }
}

