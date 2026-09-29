using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
     public AnimationClip effect;
     public string boundBone;
     public Vector3 offset;
    public float scale = 0.25f;
}
