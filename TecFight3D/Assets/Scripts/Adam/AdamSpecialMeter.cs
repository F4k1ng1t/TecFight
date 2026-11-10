using UnityEngine;
using UnityEngine.UI;

public class AdamSpecialMeter : MonoBehaviour
{
    private Image MeterUI;
    //amount of special a shine uses
    private const float shinePercent = 0.25f;
    private const float specialMax = 10f;
    public float specialCharge;
    //amount of damage required for an empowered special

    public bool UseShine()
    {
        if(specialCharge > (specialMax * shinePercent))
        {
            specialCharge -= specialCharge * shinePercent;
            UpdateUI();
            return true;
        }
        return false;
    }

    public bool UseEmpoweredSpecial()
    {
        if(specialCharge >= specialMax)
        {
            specialCharge = 0; 
            UpdateUI();
            return true;
        }
        return false;
    }

    public void AddCharge(float amount)
    {
        specialCharge = Mathf.Min(specialCharge + amount, specialMax);
        UpdateUI();
    }

    private void UpdateUI()
    {
        MeterUI.fillAmount = specialCharge / specialMax;
    }
    
    public void ConnectUI(Image ui)
    {
        MeterUI = ui;
    }
}
