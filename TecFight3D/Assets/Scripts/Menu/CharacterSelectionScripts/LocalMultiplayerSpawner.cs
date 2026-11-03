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
        SpawnPlayer(1, player1SpawnPoint, P1_SaveKey);
        SpawnPlayer(2, player2SpawnPoint, P2_SaveKey);
    }

    private void SpawnPlayer(int playerNum, Transform spawnLocation, string saveKey)
    {
        int selectedIndex = PlayerPrefs.GetInt(saveKey, playerNum == 1 ? 0 : 1);

        if (selectedIndex < characters.Length && spawnLocation != null)
        {
            GameObject prefabToSpawn = characters[selectedIndex].gameplayPrefab;

            GameObject spawnedCharacter = Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
        }
    }
}
