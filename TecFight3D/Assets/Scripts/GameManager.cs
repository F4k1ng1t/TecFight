using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    [SerializeField] int lives = 3;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject CharacterPrefab;
    Transform[] Players;
    int[] perPlayerLives;
    private void Awake()
    {
        if (gameManager != null)
        {
            Destroy(gameObject);
            return;
        }
        gameManager = this;
        //the 2 in each of these is the number of players which we are setting statically for now
        Players = new Transform[2];
        perPlayerLives = Enumerable.Repeat(lives, 2).ToArray();
        GameObject temp;
        //currently hard coded to spawn 2 of the same character in the 2 spawn points.
        //Future updates will need to spawn the amount of players in the game, and spawn the players with the correct character and attachment
        temp = Instantiate(CharacterPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
        Players[0] = temp.transform;
        temp = Instantiate(CharacterPrefab, spawnPoints[1].position, spawnPoints[1].rotation);
        Players[1] = temp.transform;
    }
    
    public void TakeLife(int playerIndex)
    {
       perPlayerLives[playerIndex] = perPlayerLives[playerIndex] - 1;
        if (perPlayerLives[playerIndex] < 1)
        {
            //end game
        }
    }
}
