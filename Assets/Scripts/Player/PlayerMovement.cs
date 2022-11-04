using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Required")]

    private CharacterController characterCont;
    private PlayerControls controls;

    [SerializeField] private float gravity;
    private Vector3 velocity;


    [Header("Move")]

    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;

    [Space]

    [SerializeField, Range(0, 1f)] private float airMultiplier;

    private Vector2 inputVector;
    private Vector3 moveDirection;

    [Space]
    


    [Header("Crouching")]

    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchYScale;
    private float startYScale;


    [Header("Slope")]

    [SerializeField] private float maxSlopeAngle;
    private float playerHeight;
    private RaycastHit slopeHit;

    private bool onSlope;
    private bool exitingSlope;


    [Header("Jump")]



    [Space]

    [SerializeField] private float jumpForce;

    private bool isGrounded;

    [Space]

    [SerializeField] private int maxDoubleJumps = 2;
    private int hasDoubleJumps;

    [Header("Redirect")]

    [SerializeField] private int maxRedirects = 2;
    private int hasRedirects;

    [Header("Grapple")]

    [Header("Required")]

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLM;
    [SerializeField] private float groundCheckRadius;



    private MovementState state;


    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    private void Awake()
    {
        characterCont = GetComponent<CharacterController>();

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Jump.performed += Jump;
        controls.Player.Redirect.performed += Redirect;
    }

    private void Start()
    {
        startYScale = transform.localScale.y;
        playerHeight = startYScale;
    }

    private void Update()
    {
        StateHandler();
        isGrounded = CheckIfGrouded();
        onSlope = IsOnSlope();

        GetInput();
        UpdateVelocity();

        Crouch();
        
        Move();
        AirMove();
    }

    private void LateUpdate()
    {
        ResetAgilityAbilities();
    }

    private void StateHandler()
    {
        if (controls.Player.Crouch.IsPressed())
        {
            if (false)
                state = MovementState.sliding;
            else
                state = MovementState.crouching;
        }
        else if (controls.Player.Sprinting.IsPressed() & ChechIfForward())
        {
            state = MovementState.sprinting;
        }
        else
        {
            state = MovementState.walking;
        }
    }


    // -------------- Movement -------------- \\


    private void GetInput()
        => inputVector = controls.Player.Move.ReadValue<Vector2>();

    private void Move()
    {
        if (!isGrounded)
            return;

        moveDirection = (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized * 
            (state == MovementState.crouching ? crouchSpeed : (state == MovementState.sprinting ? sprintSpeed : walkSpeed)); // Speed selection

        if (onSlope && !exitingSlope)
            characterCont.Move(GetSlopeMoveDir() * moveSpeed * Time.deltaTime);
        else
            
        characterCont.Move(moveDirection * Time.deltaTime);
    }

    private void AirMove()
    {
        if(isGrounded)
            return;

        moveDirection = (moveDirection + (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized * airMultiplier).normalized * moveDirection.magnitude;
        characterCont.Move(moveDirection * Time.deltaTime);
    }

    private void Crouch() 
    {
        if (controls.Player.Crouch.WasReleasedThisFrame() || !isGrounded)
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        if (state != MovementState.crouching)
            return;

        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        playerHeight = crouchYScale;
    }

    // ~~~~~ Slope

    private Vector3 GetSlopeMoveDir()
        => Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;


    // -------------- Jumping -------------- \\


    private void Jump(InputAction.CallbackContext context)
    {
        if (!isGrounded && hasDoubleJumps-- < 1)
            return;

        exitingSlope = true;

        velocity.y += jumpForce;
    }

    private void Redirect(InputAction.CallbackContext context)
    {
        if (hasRedirects-- > 0)
            moveDirection = (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized * moveDirection.magnitude;
    }

    private void UpdateVelocity()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else
            velocity.y += gravity * Time.deltaTime;

        characterCont.Move(velocity * Time.deltaTime);
    }

    private void ResetAgilityAbilities()
    {
        if (!isGrounded)
            return;

        hasDoubleJumps = maxDoubleJumps;
        hasRedirects = maxRedirects;
        exitingSlope = false;
    }

    // Bool checks

    private bool CheckIfGrouded()
        => Physics.CheckSphere(new Vector3(transform.position.x, groundCheck.position.y, transform.position.z), groundCheckRadius, groundLM, QueryTriggerInteraction.Ignore);

    private bool IsOnSlope()
    {
        if (Physics.Raycast(groundCheck.position, Vector3.down, out slopeHit, 0.4f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
            
        return false;
    }
        
    private bool ChechIfForward()
    {
        float angle = Quaternion.LookRotation(moveDirection).eulerAngles.y - orientation.eulerAngles.y;
        angle = angle < 0 ? -angle : angle; // Handmade Abs)

        if (angle < 46 || angle == 315)
            return true;

        return false;
    }
}