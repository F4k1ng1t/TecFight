using UnityEngine;

public class BlastZoneBehavior : MonoBehaviour
{

    [SerializeField] Transform respawnPoint;
    Transform[] Players;
    StageBlastZones stage;
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
            if (player.position.x > stage.SideBlastZone || player.position.x < (stage.SideBlastZone * -1) || player.position.y > stage.TopBlastZone || player.position.y < stage.BottomBlastZone)
            {
                //move player to respawn
                player.position = respawnPoint.position;
                //take life
            }
        }
    }

    public void SetPlayers(Transform[] PlayersToAdd)
    {
        Players = PlayersToAdd;
    }
}
