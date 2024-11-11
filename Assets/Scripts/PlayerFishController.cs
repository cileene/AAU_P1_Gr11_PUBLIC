using UnityEngine;
using UnityEngine.InputSystem;

//TODO: #9 Make a base CharacterController class that PlayerFishController and EnemyFishController can inherit from in compliance with OOP

public class PlayerFishController : MonoBehaviour
{
    // Here we define the variables we need
    // Private class variables used internally by the script
    // We write them with an underscore and in camelCase: _privateVariable
    private CharacterController _controller; // Reference to the CharacterController component
    private InputSystem_Actions _inputActions; // Holds the input actions defining the keys and buttons
    private Vector2 _moveInput = Vector2.zero; // Stores the current movement input
    private bool _isDashing = false; // Indicates whether the player is currently dashing
    private float _dashTimer = 0f; // Timer to track the remaining dash duration
    private Vector3 _velocity = Vector3.zero; // Stores the player's velocity, used for Gravity

    // All public class variables can be set in the Unity Editor
    // And are accessible from other scripts
    // We write them in PascalCase without an underscore: PublicVariable
    [Header("Movement Settings")]
    public float MoveSpeed = 4f; // Normal movement speed
    public float DashSpeed = 8f; // Speed during dash
    public float DashDuration = 0.2f; // Duration of the dash

    [Header("Gravity Settings")]
    public bool GravityAffectsMovement = true; // Should gravity affect the fish while moving?
    public float Gravity = -0.5f; // Simulate sinking
    public float InitialFallSpeed = -0.5f; // Initial sinking speed
    public float MaxFallSpeed = -1.5f; // Maximum fall speed due to Gravity

    // Local variables written inside methods are written in camelCase without an underscore: localVariable

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        // Initialize Input Actions
        _inputActions = new InputSystem_Actions();

        // Assign callbacks
        _inputActions.Fish.Move.performed += OnMove;
        _inputActions.Fish.Move.canceled += OnMoveCanceled;
        _inputActions.Fish.Dash.performed += OnDash;
    }

    private void OnEnable()
    {
        // Enable the input actions when the script is enabled
        _inputActions.Fish.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions when the script is disabled
        _inputActions.Fish.Disable();
    }

    private void Update()
    {
        // This method is called once per frame
        // It handles movement, dashing, and Gravity

        if (_isDashing)
        {
            // Move the character with dash speed
            MoveCharacter(dash: true);

            // Decrease the dash timer
            _dashTimer -= Time.deltaTime;

            if (_dashTimer <= 0)
            {
                // End dashing
                _isDashing = false;
            }
        }
        else if (_moveInput != Vector2.zero)
        {
            // Move the character with normal speed
            MoveCharacter();
        }
        else
        {
            // Apply Gravity to the vertical velocity
            ApplyGravity();
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Called when movement input is performed
        // Updates the moveInput variable with the current input values
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Called when movement input is canceled
        // Resets the moveInput to zero
        _moveInput = Vector2.zero;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        // Called when the dash input is performed
        if (!_isDashing && _moveInput != Vector2.zero)
        {
            // Start dashing if not already dashing and there is movement input
            _isDashing = true;
            _dashTimer = DashDuration;
            Debug.Log("Dash");
        }
    }

    // This method moves the character based on the input
    public void MoveCharacter(bool dash = false)
    {
        // We only move on the X and Y axis and set Z to 0 to make sure we don't move in the Z axis
        Vector3 move = new Vector3(_moveInput.x, _moveInput.y, 0f);

        // This is called a ternary operator which is functionally equivalent to an If/Else statement
        float speed = dash ? DashSpeed : MoveSpeed;

        // Multiply the move vector by the speed
        // This is the same as writing move = move * speed;
        move *= speed;

        if (GravityAffectsMovement)
        {
            // Include vertical velocity from Gravity
            // Same syntax as above
            move.y += _velocity.y; //TODO: #8 Figure out if this feels bad
        }

        // Making sure that we're independent of frame rate
        // This one's for you Valdemar
        _controller.Move(move * Time.deltaTime);
    }
    public void ApplyGravity()
    {
        // Apply sinking Gravity
        _velocity.y += Gravity * Time.deltaTime;
        _velocity.y = Mathf.Max(_velocity.y, MaxFallSpeed);

        // Apply vertical velocity
        // And again, we're multiplying by Time.deltaTime to make sure we're independent of frame rate
        _controller.Move(new Vector3(0f, _velocity.y, 0f) * Time.deltaTime);
    }

    // This is something we can expand later on
    // For now, it just logs a message when the player hits geometry
    // It could be used to apply damage, or to push the player back
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Geometry"))
        {
            Debug.Log("HIT Geometry");

        }
    }
}