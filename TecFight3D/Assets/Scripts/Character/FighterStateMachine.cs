using System;
using UnityEngine;

public class FighterStateMachine : MonoBehaviour
{
    //for animation - collin
    public CharacterAnimator animator;

    [HideInInspector] public Rigidbody rig;
    [HideInInspector] public FighterInput input;
    [HideInInspector] public FighterInputProcesser fip;

    public CharacterObject charObj;
    public int direction = 1;

    public IFighterState currentState;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        input = GetComponent<FighterInput>();
        fip = GetComponent<FighterInputProcesser>();
        animator = gameObject.GetComponentInChildren<CharacterAnimator>();

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
    public void SetDirection(bool right)
    {
        if(right)
        {
            direction = 1;
            animator.SetVisualDirection(direction);
        }
        else
        {
            direction = -1;
            animator.SetVisualDirection(direction);
        }
    }
    public void FlipDirection()
    {
        direction *= -1;
    }
}
