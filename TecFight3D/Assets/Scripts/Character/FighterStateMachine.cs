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
        Debug.Log(newState.GetType().Name);
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
}
