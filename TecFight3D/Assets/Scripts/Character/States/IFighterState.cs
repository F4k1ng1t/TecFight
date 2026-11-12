using System.Linq.Expressions;
using UnityEngine;

public interface IFighterState
{
    void Enter(FighterStateMachine f)
    {
        Enter(f, null);
    }
    void Enter(FighterStateMachine f, AttackObject a)
    {
        Enter(f);
    }
    void Exit();
    void Update();
    void FixedUpdate();
}
