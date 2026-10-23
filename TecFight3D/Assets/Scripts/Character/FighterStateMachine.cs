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
    public FighterECB ecb;
    [Tooltip("the character's attributes.")]
    public CharacterObject charObj;
    public int direction = 1;
    public int currentDJC;
    public bool airMovementEnabled = true;

    public IFighterState currentState;
    //sheild stuff
    [HideInInspector] public bool isSheilding = false;
    [HideInInspector] public bool isDodging = false;
    [HideInInspector] public float sheildHealth;
    [HideInInspector] public float timeSinceSheild;
    public GameObject sheild;

    void Start()
    {
        sheildHealth = charObj.shieldHealth;
        timeSinceSheild = Time.time; //all of this stuff can probably be moved into its own script if its annoying
        sheild.SetActive(false);

        rig = GetComponent<Rigidbody>();
        input = GetComponent<FighterInput>();
        fip = GetComponent<FighterInputProcesser>();
        gravity = GetComponent<Gravity>();
        animator = gameObject.GetComponentInChildren<CharacterAnimator>();
        getAttack = GetComponent<GetAttackObject>();
        currentDJC = charObj.doubleJumpCount;
        ecb = GetComponent<FighterECB>();
        //SetState(new AirState());
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Update();
    }
    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
    public void SetState(IFighterState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        if(newState is AirState)
        {
            //Debug.Log("to air from: " + currentState);
        }
        currentState = newState;
        currentState.Enter(this);
        Debug.Log(newState.GetType().Name);
    }
    public void SetStateAttack(AttackObject attack)
    {
        if (attack == null)
        {
            Debug.LogError("SetStateAttack received a null AttackObject.", this);
            return;
        }

        if (currentState != null)
            currentState.Exit();

        AttackState newState = new AttackState();
        currentState = newState;
        newState.Enter(this, attack);

        Debug.Log($"Entered AttackState with attack: {attack.attackName}");
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
        float inputX = input.MoveInput.x;
        float currentVelocity = gravity.horizontalVelocity;
        if (airMovementEnabled)
        {
            if (Mathf.Abs(currentVelocity) < charObj.airSpeed)
            {
                gravity.horizontalVelocity += inputX * charObj.airAccel * Time.fixedDeltaTime;
                gravity.horizontalVelocity = Mathf.Clamp(
                    gravity.horizontalVelocity,
                    -charObj.airSpeed,
                    charObj.airSpeed
                );
            }
            else if (fip.AttemptMoveOpposite())
            {
                gravity.horizontalVelocity += inputX * charObj.airAccel * Time.fixedDeltaTime;
            }

            if (fip.FastFall())
            {
                gravity.verticalVelocity = Mathf.Max(
                    gravity.verticalVelocity,
                    -charObj.fastfallSpeed
                );
            }
        }
            
    }


}
