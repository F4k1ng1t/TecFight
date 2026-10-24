
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.Scripting.APIUpdating;

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
                f.SetDirection(f.input.MoveInput.x);
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
        if ((f.currentState is RunState || f.currentState is InitialDashState))
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
        if(f.currentState is KnockdownState)
        {
            return attackObjects["GetupAttack"];
        }

        return null;
    }
    public AttackObject GetSpecial()
    {
        //do we need this? (redundant with the state transition check)
        if (f.currentState is IdleState || f.currentState is AirState || f.currentState is RunState || f.currentState is WalkState) //Make an actionability variable instead of this brute force method
        {
            if (Mathf.Abs(f.input.MoveInput.x) > 0.5f)
            {
                f.SetDirection(f.input.MoveInput.x);
                return attackObjects["SideSpecial"];
            }
            if (f.input.MoveInput.y > 0.5f)
            {
                return attackObjects["UpSpecial"];
            }
            if (f.input.MoveInput.y < -0.5f)
            {
                return attackObjects["DownSpecial"];
            }
            return attackObjects["NeutralSpecial"];
        }
        return null;
    }
    public AttackObject GetStrong()
    {
        if (f.currentState is IdleState || f.currentState is WalkState)
        {
            if (Mathf.Abs(f.input.MoveInput.x) > 0.5f)
            {
                f.SetDirection(f.input.MoveInput.x);
                return attackObjects["ForwardStrong"];
            }
            if (f.input.MoveInput.y > 0.5f)
            {
                return attackObjects["UpStrong"];
            }
            if (f.input.MoveInput.y < -0.5f)
            {
                return attackObjects["DownStrong"];
            }
            return attackObjects["ForwardStrong"];
        }
        return null;
    }
}
