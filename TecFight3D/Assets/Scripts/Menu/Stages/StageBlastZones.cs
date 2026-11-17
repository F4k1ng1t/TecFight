using UnityEngine;

[CreateAssetMenu(fileName = "StageBlastZones", menuName = "Scriptable Objects/StageBlastZones")]
public class StageBlastZones : ScriptableObject
{
    public float TopBlastZone = 12f;
    public float BottomBlastZone = -2f;
    public float SideBlastZone = 14f;
}
