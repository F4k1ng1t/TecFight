using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    [Header("UI References")]
    public Image previewImageDisplay;
    public Button playButton;         

    [System.Serializable]
    public struct MapData
    {
        public string mapDisplayName;    
        public string unitySceneName;    
        public Sprite mapPreviewSprite;
    }

    [Header("Map Configurations")]
    public MapData[] availableMaps;

    private string sceneToLoad = "";

    void Start()
    {
        if (previewImageDisplay != null) previewImageDisplay.gameObject.SetActive(false);
        if (playButton != null) playButton.interactable = false;
    }

    public void SelectMap(int arrayIndex)
    {
        if (arrayIndex < 0 || arrayIndex >= availableMaps.Length) return;

        MapData selectedMap = availableMaps[arrayIndex];
        sceneToLoad = selectedMap.unitySceneName;

        if (previewImageDisplay != null)
        {
            previewImageDisplay.gameObject.SetActive(true);
            previewImageDisplay.sprite = selectedMap.mapPreviewSprite;
        }

        if (playButton != null) playButton.interactable = true;
    }


    public void ConfirmLoadMap()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
