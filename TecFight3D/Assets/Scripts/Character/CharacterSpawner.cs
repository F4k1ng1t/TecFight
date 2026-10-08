using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject CharacterPrefab;
    private void Awake()
    {
        //currently hard coded to spawn 2 of the same character in the 2 spawn points.
        //Future updates will need to spawn the amount of players in the game, and spawn the players with the correct character and attachment
        Instantiate(CharacterPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
        Instantiate(CharacterPrefab, spawnPoints[1].position, spawnPoints[1].rotation);
    }
}
