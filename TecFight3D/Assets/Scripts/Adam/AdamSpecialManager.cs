using UnityEngine;
using UnityEngine.UI;

public class AdamSpecialManager : MonoBehaviour, ICharacterSpecialManager
{
    [SerializeField] private FighterStateMachine fsm;
    [SerializeField] private AdamSpecialMeter meter;

    private void Start()
    {
        if (!fsm)
        {
            gameObject.GetComponent<FighterStateMachine>();
        }
        if (!meter)
        {
            gameObject.GetComponent<AdamSpecialMeter>();
        }
        fsm.onDamageTaken.AddListener(meter.AddCharge);
    }
    public IFighterState GetSpecial()
    {
        if (Mathf.Abs(fsm.input.MoveInput.x) > 0.5f)
        {
            fsm.SetDirection(fsm.input.MoveInput.x);
            if (meter.UseEmpoweredSpecial())
            {
                return new AdamEmpSSpecialState();
            }
            return new AdamSSpecialState();
        }
        if (fsm.input.MoveInput.y > 0.5f)
        {
            if (meter.UseEmpoweredSpecial())
            {
                return new AdamEmpUSpecialState();
            }
            return new AdamUSpecialState();
        }
        if (fsm.input.MoveInput.y < -0.5f)
        {
            return new AdamDSpecialState();
        }
        if (meter.UseEmpoweredSpecial())
        {
            return new AdamEmpNSpecialState();
        }
        if (meter.UseShine())
        {
            return new AdamNSpecialState();
        }
        //this aint right but you know
        return fsm.ecb.IsGrounded ? new IdleState() : new AirState();
    }

    public void ConnectUI(Image characterUIElement)
    {
        meter.ConnectUI(characterUIElement);
    }
}
