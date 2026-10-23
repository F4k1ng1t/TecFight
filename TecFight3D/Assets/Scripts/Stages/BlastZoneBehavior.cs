using UnityEngine;

public class BlastZoneBehavior : MonoBehaviour
{

    [SerializeField] Transform respawnPoint;
    [SerializeField] StageBlastZones stage;
    Transform[] Players;
    int i;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        i = 0;
        //the easiest solution
        foreach(Transform player in Players)
        {
            //Debug.Log("Checked: " + player.name + " X: " + player.position.x + " Y: " + player.position.y);
            if (player.position.x > stage.SideBlastZone || player.position.x < (stage.SideBlastZone * -1) || player.position.y > stage.TopBlastZone || player.position.y < stage.BottomBlastZone)
            {
                //move player to respawn
                player.position = respawnPoint.position;
                //take life
                GameManager.gameManager.TakeLife(i);
            }
            i++;
        }
    }

    public void SetPlayers(Transform[] PlayersToAdd)
    {
        Players = PlayersToAdd;
    }
}
