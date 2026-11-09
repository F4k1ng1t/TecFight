using UnityEngine;

public class AdamSpecialMeter : MonoBehaviour
{
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
            return true;
        }
        return false;
    }

    public bool UseEmpoweredSpecial()
    {
        if(specialCharge >= specialMax)
        {
            specialCharge = 0;
            return true;
        }
        return false;
    }
}
