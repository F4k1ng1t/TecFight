using UnityEngine;

[CreateAssetMenu(fileName = "AttackObject", menuName = "Scriptable Objects/AttackObject")]
public class AttackObject : ScriptableObject
{
    public string attackName;
    public string animationName;
    public int startup;
    public int activeFrames;
    public int endlag;
    public IAttackBehaviour attackBehaviour;


}
