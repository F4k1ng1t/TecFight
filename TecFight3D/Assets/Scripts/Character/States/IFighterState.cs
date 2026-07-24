using System.Linq.Expressions;
using UnityEngine;

public interface IFighterState
{
    void Enter(FighterStateMachine f);
    void Exit();
    void Update();
    void FixedUpdate();
}
