using System;
using UnityEngine;

public class FighterStateMachine : MonoBehaviour
{
    //for animation - collins
    public CharacterAnimator animator;

    public Rigidbody rig;
    public FighterInput input;
    public FighterInputProcesser fip;
    public Gravity gravity;
    public GetAttackObject getAttack;
    [Tooltip("the character's attributes.")]
    public CharacterObject charObj;
    public int direction = 1;
    public int currentDJC;

    public IFighterState currentState;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        input = GetComponent<FighterInput>();
        fip = GetComponent<FighterInputProcesser>();
        gravity = GetComponent<Gravity>();
        animator = gameObject.GetComponentInChildren<CharacterAnimator>();
        getAttack = GetComponent<GetAttackObject>();
        currentDJC = charObj.doubleJumpCount;
        SetState(new AirState());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
    }
    void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
    public void SetState(IFighterState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
        //Debug.Log(newState.GetType().Name);
    }
    public void SetStateAttack(AttackObject attack)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = new AttackState();
        currentState.Enter(this, attack);
        Debug.Log(currentState.GetType().Name);
    }
    public void SetDirection(float newDirection)
    {
        direction = newDirection > 0 ? 1 : -1;
        animator.SetVisualDirection(direction);
    }
    public void FlipDirection()
    {
        direction *= -1;
        animator.SetVisualDirection(direction);
    }

    public void ApplyAirMovement()
    {
        float currentVelocity = rig.linearVelocity.x;

        if (Mathf.Abs(currentVelocity) < charObj.airSpeed)
        {
            rig.AddForce(input.MoveInput.x * charObj.airAccel, 0, 0, ForceMode.Acceleration);
            rig.linearVelocity = new Vector3(Mathf.Clamp(rig.linearVelocity.x, -charObj.airSpeed, charObj.airSpeed), rig.linearVelocity.y, rig.linearVelocity.z);
        }
        else if (fip.AttemptMoveOpposite())
        {
            rig.linearVelocity = new Vector3(Mathf.Clamp(currentVelocity, -charObj.airSpeed, charObj.airSpeed), rig.linearVelocity.y, rig.linearVelocity.z);
            rig.AddForce(input.MoveInput.x * charObj.airAccel, 0, 0, ForceMode.Acceleration);
        }
        else
        {
            rig.linearVelocity = new Vector3(currentVelocity, rig.linearVelocity.y, rig.linearVelocity.z);
        }

        if (fip.FastFall())
        {
            rig.AddForce(0, -charObj.fastfallAccel, 0, ForceMode.Acceleration);
            rig.linearVelocity = new Vector3(rig.linearVelocity.x, Mathf.Max(rig.linearVelocity.y, -charObj.fastfallSpeed), rig.linearVelocity.z);
        }
    }
}
