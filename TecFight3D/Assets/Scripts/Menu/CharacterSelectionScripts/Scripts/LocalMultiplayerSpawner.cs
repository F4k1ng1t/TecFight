using UnityEngine;

public class LocalMultiplayerSpawner : MonoBehaviour
{
    public CharacterData[] characters;

    [Header("Spawn Locations")]
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    private const string P1_SaveKey = "P1_SelectedCharacter";
    private const string P2_SaveKey = "P2_SelectedCharacter";

    void Start()
    {
        int p1Index = PlayerPrefs.GetInt(P1_SaveKey, 0);
        int p2Index = PlayerPrefs.GetInt(P2_SaveKey, 1);

        GameObject p1Instance = Instantiate(characters[p1Index].gameplayPrefab, player1SpawnPoint.position, player1SpawnPoint.rotation);
        GameObject p2Instance = Instantiate(characters[p2Index].gameplayPrefab, player2SpawnPoint.position, player2SpawnPoint.rotation);

        if (p1Index == p2Index)
        {
            ApplyMirrorMatchTint(p2Instance);
        }
    }

    private void ApplyMirrorMatchTint(GameObject player2Object)
    {
        Renderer[] renderers = player2Object.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    mat.color = new Color(0.6f, 0.6f, 1.0f);
                    mat.color = new Color(0.6f, 0.6f, 1.0f);
                }
            }
        }
    }
}
