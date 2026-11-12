
using System;
using UnityEngine;
using UnityEngine.Events;

public class FighterStateMachine : MonoBehaviour
{
    // For animation - Collins
    public CharacterAnimator animator;

    public Rigidbody rig;
    public FighterInput input;
    public FighterInputProcesser fip;
    public FighterPhysics physics;
    public GetAttackObject getAttack;
    public FighterECB ecb;
    public FighterAudio fighterAudio;
    [Tooltip("the character's attributes.")]
    public CharacterObject charObj;

    public int direction = 1;
    public int currentDJC;
    public bool airMovementEnabled = true;
    public bool hasAirdodge = false;

    public IFighterState currentState;
    public ICharacterSpecialManager specialManager;

    // Shield stuff
    [HideInInspector] public bool isSheilding = false;
    [HideInInspector] public bool isDodging = false;
    [HideInInspector] public float sheildHealth;
    [HideInInspector] public float timeSinceSheild;

    //events for per character specials (might need more for other characters)
    public UnityEvent<float> onDamageTaken;

    public GameObject sheild;

    private void Start()
    {
        sheildHealth = charObj.shieldHealth;
        timeSinceSheild = Time.time;

        sheild.SetActive(false);

        rig = GetComponent<Rigidbody>();
        input = GetComponent<FighterInput>();
        fip = GetComponent<FighterInputProcesser>();
        physics = GetComponent<FighterPhysics>();
        animator = GetComponentInChildren<CharacterAnimator>();
        getAttack = GetComponent<GetAttackObject>();
        currentDJC = charObj.doubleJumpCount;
        ecb = GetComponent<FighterECB>();
        if(!fighterAudio)
            fighterAudio = gameObject.GetComponent<FighterAudio>();
        if (specialManager == null)
            specialManager = gameObject.GetComponent<ICharacterSpecialManager>();
        //SetState(new AirState());
    }

    private void Update()
    {
        currentState?.Update();
    }

    private void FixedUpdate()
    {
        // 1. Let the current state update its movement and velocity.
        currentState?.FixedUpdate();

        // 2. Apply physics after the state has finished.
        physics?.Simulate();
    }

    public void SetState(IFighterState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter(this);
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
        if (!airMovementEnabled || physics == null)
            return;

        float inputX = input.MoveInput.x;
        float currentVelocity = physics.Velocity.x;

        if (Mathf.Abs(currentVelocity) < charObj.airSpeed)
        {
            physics.SetHorizontalVelocity(
                Mathf.Clamp(
                    currentVelocity + inputX * charObj.airAccel * Time.fixedDeltaTime,
                    -charObj.airSpeed,
                    charObj.airSpeed
                )
            );
        }
        else if (fip.AttemptMoveOpposite())
        {
            physics.SetHorizontalVelocity(
                currentVelocity + inputX * charObj.airAccel * Time.fixedDeltaTime
            );
        }

        if (fip.FastFall() && physics.Velocity.y > -charObj.fastfallSpeed)
        {
            physics.SetVerticalVelocity(-charObj.fastfallSpeed);
        }
    }
}
