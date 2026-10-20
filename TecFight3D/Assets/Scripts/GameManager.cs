using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    [SerializeField] int lives = 3;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform[] lifeUIHLG; //you don't need to know what this is, right?
    [SerializeField] GameObject lifePrefab;
    [SerializeField] Image[] healthBars;
    [SerializeField] GameObject CharacterPrefab;
    [SerializeField] BlastZoneBehavior blastZones;
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
        foreach(Transform hlg in lifeUIHLG)
        {
            for(int i = 0; i < lives; i++)
            {
                Instantiate(lifePrefab, hlg);
            }
        }

        GameObject temp;
        //currently hard coded to spawn 2 of the same character in the 2 spawn points.
        //Future updates will need to spawn the amount of players in the game, and spawn the players with the correct character and attachment
        temp = Instantiate(CharacterPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
        temp.GetComponent<KnockbackHandler>().SetHealthBar(healthBars[0]);
        Players[0] = temp.transform;

        temp = Instantiate(CharacterPrefab, spawnPoints[1].position, spawnPoints[1].rotation);
        temp.GetComponent<KnockbackHandler>().SetHealthBar(healthBars[1]);
        Players[1] = temp.transform;
    }

    private void Start()
    {
        if (blastZones == null)
        {
            blastZones = FindAnyObjectByType<BlastZoneBehavior>();
        }
        blastZones.SetPlayers(Players);
    }

    public void TakeLife(int playerIndex)
    {
        perPlayerLives[playerIndex] = perPlayerLives[playerIndex] - 1;
        //ui update
        Destroy(lifeUIHLG[playerIndex].GetChild(0).gameObject);
        if (perPlayerLives[playerIndex] < 1)
        {
            //end game
            SceneManager.LoadScene("WinScreen");
        }
    }
}
