using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class FighterCameraTargetGroup : MonoBehaviour
{
    private CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    public void SetPlayers(Transform[] players)
    {
        foreach (Transform player in players)
        {
            if (player == null)
                continue;

            targetGroup.AddMember(player, 1f, 1f);
        }
    }
}