using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character Selection/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterIcon;          
    public GameObject previewModelPrefab;
    public GameObject gameplayPrefab;     
}
