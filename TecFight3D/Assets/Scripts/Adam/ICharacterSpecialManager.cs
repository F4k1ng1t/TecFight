using UnityEngine;
using UnityEngine.UI;

public interface ICharacterSpecialManager
{
    public IFighterState GetSpecial();
    public void ConnectUI(Image characterUIElement);
}
