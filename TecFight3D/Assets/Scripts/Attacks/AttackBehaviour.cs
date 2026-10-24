
using UnityEngine;

public abstract class AttackBehaviour : ScriptableObject
{
    public virtual bool CanExit => true;
    public virtual bool ControlsAirMovement => false;

    public virtual void OnAttackEnter(FighterStateMachine fsm, AttackObject attack) { }
    public virtual void OnAttackUpdate(FighterStateMachine fsm, AttackObject attack) { }
    public virtual void OnAttackFixedUpdate(FighterStateMachine fsm, AttackObject attack) { }
    public virtual void OnAttackExit(FighterStateMachine fsm, AttackObject attack) { }
    public virtual void OnRecoil(FighterStateMachine fsm, AttackObject attack) { }
}
