
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class GetAttackObject : MonoBehaviour
{
    FighterStateMachine f;

    [SerializedDictionary("Attack Name", "Attack Object")]
    public SerializedDictionary<string, AttackObject> attackObjects = new SerializedDictionary<string, AttackObject>();

    private void Start()
    {
        f = GetComponent<FighterStateMachine>();
    }

    public AttackObject GetAttack()
    {

        if (f.currentState is IdleState || f.currentState is WalkState)
        {
            if (Mathf.Abs(f.input.MoveInput.x) > 0.5f)
            {
                return attackObjects["ForwardTilt"];
            }
            if (f.input.MoveInput.y < -0.5f)
            {
                return attackObjects["DownTilt"];
            }
            if (f.input.MoveInput.y > 0.5f)
            {
                return attackObjects["UpTilt"];
            }
            return attackObjects["Jab1"];
        }
        if ((f.currentState is RunState || f.currentState is InitialDashState) && f.input.Attack)
        {
            return attackObjects["DashAttack"];
        }

        if (f.currentState is AirState)
        {
            if (f.input.MoveInput.x * f.direction < -0.5f)
            {
                return attackObjects["BackAir"];
            }
            if (Mathf.Abs(f.input.MoveInput.x) > 0.5f)
            {
                return attackObjects["ForwardAir"];
            }
            if (f.input.MoveInput.y < -0.5f)
            {
                return attackObjects["DownAir"];
            }
            if (f.input.MoveInput.y > 0.5f)
            {
                return attackObjects["UpAir"];
            }
            return attackObjects["NeutralAir"];
        }


        return null;
    }
    //public SpecialObject GetSpecial()
    //{

    //}
    //public StrongObject GetStrong()
    //{

    //}
}
