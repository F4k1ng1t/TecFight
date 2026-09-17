using UnityEngine;
using UnityEngine.InputSystem;

public class FighterInput : MonoBehaviour
{
    private InputSystem_Actions controls;
    public Vector2 MoveInput { get; private set; }

    public bool IsHoldingJump { get; private set; }

    public bool Shorthop {  get; private set; }

    public bool Fullhop { get; private set; }

    public bool LightAttack;

    // One-shot event. Remains true until consumed.
    public bool Flick { get; private set; }

    // Normalized cardinal direction of the detected flick.
    // Right = (1, 0)
    // Left  = (-1, 0)
    // Up    = (0, 1)
    // Down  = (0, -1)
    public Vector2 FlickDirection { get; private set; }

    [Header("Stick")]
    [SerializeField] private float deadzone = 0.15f;

    [Header("Flick Detection")]
    [SerializeField] private float flickThreshold = 0.70f;

    // Minimum amount the stick must move during the flick.
    [SerializeField] private float flickDistance = 0.50f;

    // Maximum time allowed for the movement.
    [SerializeField] private float flickWindow = 0.10f;


    // Prevents the same direction from producing repeated flicks
    // while the stick is being held.
    private enum StickDirection
    {
        Neutral,
        Left,
        Right,
        Up,
        Down
    }

    private Vector2 previousInput;
    private float previousInputTime;

    private StickDirection previousSignificantDirection;
    private StickDirection currentSignificantDirection;

    private void Awake()
    {
        controls = new InputSystem_Actions();

        previousInput = Vector2.zero;
        previousInputTime = Time.time;

        previousSignificantDirection = StickDirection.Neutral;
        currentSignificantDirection = StickDirection.Neutral;
    }

    private void OnEnable()
    {
        controls.Fighter.Move.performed += OnMove;
        controls.Fighter.Move.canceled += OnMove;
        controls.Fighter.Jump.started += OnJumpStarted;
        controls.Fighter.Jump.performed += OnJumpPerformed;
        controls.Fighter.Jump.canceled += OnJumpCanceled;
        controls.Fighter.Attack.performed += OnLightAttack;

        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Fighter.Move.performed -= OnMove;
        controls.Fighter.Move.canceled -= OnMove;
        controls.Fighter.Jump.started -= OnJumpStarted;
        controls.Fighter.Jump.performed -= OnJumpPerformed;
        controls.Fighter.Jump.canceled -= OnJumpCanceled;
        controls.Fighter.Attack.performed -= OnLightAttack;

        controls.Disable();
    }

    private void Update()
    {
        DetectFlick();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        IsHoldingJump = true;
    }
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        IsHoldingJump = true;
    }
    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        IsHoldingJump = false;
    }
    private void OnLightAttack(InputAction.CallbackContext ctx)
    {
        LightAttack = true;
    }

    private void DetectFlick()
    {
        Vector2 currentInput = ApplyDeadzone(MoveInput);

        float currentTime = Time.time;

        // How much the stick moved since the previous sample.
        Vector2 delta = currentInput - previousInput;

        float deltaTime = currentTime - previousInputTime;

        // Avoid division by zero.
        if (deltaTime <= 0f)
        {
            previousInput = currentInput;
            previousInputTime = currentTime;
            return;
        }

        StickDirection newDirection = GetSignificantDirection(currentInput);

        // Direction hasn't changed.
        if (newDirection == currentSignificantDirection)
        {
            previousInput = currentInput;
            previousInputTime = currentTime;
            return;
        }

        StickDirection oldDirection = currentSignificantDirection;

        currentSignificantDirection = newDirection;

        // Ignore entering neutral.
        if (newDirection == StickDirection.Neutral)
        {
            previousInput = currentInput;
            previousInputTime = currentTime;
            return;
        }

        /*
         * We only care about a directional transition that happens
         * quickly enough and moves far enough.
         *
         * Examples:
         *
         * Neutral -> Right
         * Left    -> Right
         * Neutral -> Up
         * Down    -> Up
         */

        bool cameFromNeutral =
            oldDirection == StickDirection.Neutral;

        bool cameFromOpposite =
            IsOpposite(oldDirection, newDirection);

        // Check how long it took to make the movement.
        float movementTime = currentTime - previousInputTime;

        // Check how far the stick moved.
        float movementAmount = delta.magnitude;

        /*
         * A direct analog movement must satisfy both conditions.
         *
         * This prevents slowly moving the stick across the threshold
         * from being interpreted as a flick.
         */
        if ((cameFromNeutral || cameFromOpposite) &&
            movementTime <= flickWindow &&
            movementAmount >= flickDistance)
        {
            TriggerFlick(newDirection);
        }

        previousSignificantDirection = oldDirection;

        previousInput = currentInput;
        previousInputTime = currentTime;
    }

    private Vector2 ApplyDeadzone(Vector2 input)
    {
        float magnitude = input.magnitude;

        if (magnitude <= deadzone)
            return Vector2.zero;

        // Remap the stick outside the deadzone to 0-1.
        float normalizedMagnitude =
            Mathf.InverseLerp(deadzone, 1f, magnitude);

        return input.normalized * normalizedMagnitude;
    }

    private StickDirection GetSignificantDirection(Vector2 input)
    {
        float absX = Mathf.Abs(input.x);
        float absY = Mathf.Abs(input.y);

        /*
         * Below the flick threshold means the stick hasn't committed
         * strongly enough to a direction yet.
         */
        if (absX < flickThreshold && absY < flickThreshold)
            return StickDirection.Neutral;

        // Pick the dominant axis.
        if (absX > absY)
        {
            return input.x > 0f
                ? StickDirection.Right
                : StickDirection.Left;
        }

        return input.y > 0f
            ? StickDirection.Up
            : StickDirection.Down;
    }

    private bool IsOpposite(
        StickDirection a,
        StickDirection b)
    {
        return
            (a == StickDirection.Left &&
             b == StickDirection.Right) ||

            (a == StickDirection.Right &&
             b == StickDirection.Left) ||

            (a == StickDirection.Up &&
             b == StickDirection.Down) ||

            (a == StickDirection.Down &&
             b == StickDirection.Up);
    }

    private void TriggerFlick(StickDirection direction)
    {
        Flick = true;
        Debug.Log(direction.ToString());
        switch (direction)
        {
            case StickDirection.Left:
                FlickDirection = Vector2.left;
                break;

            case StickDirection.Right:
                FlickDirection = Vector2.right;
                break;

            case StickDirection.Up:
                FlickDirection = Vector2.up;
                break;

            case StickDirection.Down:
                FlickDirection = Vector2.down;
                break;
        }
    }

    public void ConsumeFlick()
    {
        Flick = false;
        FlickDirection = Vector2.zero;
    }

    private void LateUpdate()
    {
        Flick = false;
        FlickDirection = Vector2.zero;
    }
}
