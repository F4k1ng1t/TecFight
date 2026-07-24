using UnityEngine;
using UnityEngine.InputSystem;

public class FighterInput : MonoBehaviour
{
    private InputSystem_Actions controls;

    public Vector2 MoveInput { get; private set; }

    public bool JumpPressed { get; private set; }

    public bool Smash { get; private set; }

    public int SmashDirection { get; private set; }

    [Header("Smash Detection")]
    [SerializeField] private float smashThreshold = 0.8f;
    [SerializeField] private float neutralThreshold = 0.2f;
    [SerializeField] private float flickWindow = 0.05f;

    private float neutralTimer;
    private bool smashConsumed;

    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Fighter.Move.performed += OnMove;
        controls.Fighter.Move.canceled += OnMove;

        controls.Fighter.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        controls.Disable();

        controls.Fighter.Move.performed -= OnMove;
        controls.Fighter.Move.canceled -= OnMove;

        controls.Fighter.Jump.performed -= OnJump;
    }

    private void Update()
    {
        DetectSmashInput();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void DetectSmashInput()
    {
        Smash = false;

        bool isNeutral =
            Mathf.Abs(MoveInput.x) < neutralThreshold;

        if (isNeutral)
        {
            neutralTimer = 0f;
            smashConsumed = false;
        }
        else
        {
            neutralTimer += Time.deltaTime;
        }

        bool recentlyNeutral =
            neutralTimer <= flickWindow;

        bool strongInput =
            Mathf.Abs(MoveInput.x) >= smashThreshold;

        if (!smashConsumed &&
            recentlyNeutral &&
            strongInput)
        {
            Smash = true;
            smashConsumed = true;
            SmashDirection = MoveInput.x > 0 ? 1 : -1;

            Debug.Log("Smash Input!");
        }
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        JumpPressed = true;
    }

    private void LateUpdate()
    {
        JumpPressed = false;
        //Smash = false;
    }
    public void ConsumeSmash()
    {
        Smash = false;
    }
}