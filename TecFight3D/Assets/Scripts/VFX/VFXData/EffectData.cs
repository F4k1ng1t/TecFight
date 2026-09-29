using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
     public GameObject effect;
     public Transform boundBone;
     public Vector3 offset;
}
