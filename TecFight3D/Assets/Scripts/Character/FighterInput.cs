using System.Collections.Generic;
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

    private Queue<float> inputHistory = new Queue<float>();

    [SerializeField] private int historyFrames = 6;

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

        float currentX = MoveInput.x;
        //Debug.Log(currentX);
        if (!smashConsumed)
        {
            bool strongInput =
                Mathf.Abs(currentX) >= smashThreshold;

            if (strongInput)
            {
                int currentDirection = currentX > 0 ? 1 : -1;

                bool hadNeutral = false;
                bool hadOppositeDirection = false;

                // Check previous frames
                foreach (float previousX in inputHistory)
                {
                    // Recently came from neutral
                    if (Mathf.Abs(previousX) < neutralThreshold)
                    {
                        hadNeutral = true;
                    }
                    // Recently came from the opposite direction
                    else if (Mathf.Sign(previousX) != currentDirection)
                    {
                        hadOppositeDirection = true;
                    }
                }

                // Strong input after neutral OR a directional change
                if (hadNeutral || hadOppositeDirection)
                {
                    Smash = true;
                    smashConsumed = true;
                    SmashDirection = currentDirection;

                    Debug.Log($"Smash Input: {SmashDirection}");
                }
            }
        }

        // Add current input AFTER checking previous frames
        inputHistory.Enqueue(currentX);
        Debug.Log(inputHistory.Peek());

        // Keep only the most recent frames
        while (inputHistory.Count > historyFrames)
        {
            inputHistory.Dequeue();
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