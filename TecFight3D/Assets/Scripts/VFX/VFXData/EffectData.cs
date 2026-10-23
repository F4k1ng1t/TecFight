using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
     public AnimationClip effect;
    public bool bound = false;
     public Vector3 offset;
    public Vector3 rotation;
    public float scale = 0.25f;
}
