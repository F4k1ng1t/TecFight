
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField] int lives = 3;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform[] UISpawnPoints;
    [SerializeField] GameObject CharacterPrefab;
    [SerializeField] BlastZoneBehavior blastZones;
    [SerializeField] AudioClip deathSound;
    private AudioSource audioSource;
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

        // The 2 in each of these is the number of players.
        Players = new Transform[2];
        perPlayerLives = Enumerable.Repeat(lives, 2).ToArray();

            

        GameObject temp;
        GameObject tempUI;

        // Currently hard coded to spawn 2 of the same character.
        temp = Instantiate(CharacterPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
        tempUI = Instantiate(temp.GetComponent<FighterStateMachine>().charObj.UI, UISpawnPoints[0]);
        temp.GetComponent<KnockbackHandler>().SetHealthBar(tempUI.GetComponent<CharacterUIData>().HealthBar);
        temp.GetComponent<ICharacterSpecialManager>().ConnectUI(tempUI.GetComponent<CharacterUIData>().CharacterSpecificUI);
        temp.GetComponent<AttachmentHandler>().SetAttachment(AttachmentTransfer.p1Attachment);
        Players[0] = temp.transform;
        for (int i = 0; i < lives; i++)
        {
            Instantiate(temp.GetComponent<FighterStateMachine>().charObj.stockPrefab, tempUI.GetComponent<CharacterUIData>().StockHLG);
        }

        temp = Instantiate(CharacterPrefab, spawnPoints[1].position, spawnPoints[1].rotation);
        tempUI = Instantiate(temp.GetComponent<FighterStateMachine>().charObj.UI, UISpawnPoints[1]);
        temp.GetComponent<KnockbackHandler>().SetHealthBar(tempUI.GetComponent<CharacterUIData>().HealthBar);
        temp.GetComponent<ICharacterSpecialManager>().ConnectUI(tempUI.GetComponent<CharacterUIData>().CharacterSpecificUI);
        temp.GetComponent<AttachmentHandler>().SetAttachment(AttachmentTransfer.p2Attachment);
        tempUI.transform.localScale = new Vector2(
            tempUI.transform.localScale.x * -1,
            tempUI.transform.localScale.y
            ); 
        Players[1] = temp.transform;
        for (int i = 0; i < lives; i++)
        {
            Instantiate(temp.GetComponent<FighterStateMachine>().charObj.stockPrefab, tempUI.GetComponent<CharacterUIData>().StockHLG);
        }
    }

    private void Start()
    {
        if (blastZones == null)
        {
            blastZones = FindAnyObjectByType<BlastZoneBehavior>();
        }
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        blastZones.SetPlayers(Players);
    }

    public void TakeLife(int playerIndex)
    {
        audioSource.PlayOneShot(deathSound);
        perPlayerLives[playerIndex] = perPlayerLives[playerIndex] - 1;
        //ui update
        Players[playerIndex].gameObject.GetComponent<KnockbackHandler>().ResetDamage();

        FighterStateMachine f = Players[playerIndex].GetComponent<FighterStateMachine>();

        // Reset all movement velocity.
        f.physics.ClearVelocity();

        // Restore gravity in case the player was in a special move.
        f.physics.useGravity = true;

        f.SetState(new AirState());

        // Update lives UI.
        Destroy(UISpawnPoints[playerIndex].GetComponentInChildren<CharacterUIData>().StockHLG.GetChild(0).gameObject);

        if (perPlayerLives[playerIndex] < 1)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
