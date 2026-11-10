using UnityEngine;
using TMPro;

public class WinScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winText.text = "Player " + GameStats.winner + " Wins!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
