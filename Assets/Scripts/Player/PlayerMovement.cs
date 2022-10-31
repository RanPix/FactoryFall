using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private float maxSpeed;

    [Space]

    [SerializeField] private float airMultiplier;

    private Vector2 inputVector;
    private Vector3 moveDirection;

    [Space]
    [SerializeField] private Transform orientation;

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

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask groundLM;

    private bool isGrounded;
    private bool canJump;
    private bool hasDoubleJump;

    [Space]

    [SerializeField] private float jumpForce;



    [SerializeField] private MovementState state;
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
    }

    private void Start()
    {
        startYScale = transform.localScale.y;
        playerHeight = startYScale;
    }

    private void Update()
    {
        StateHandlerr();
        isGrounded = CheckIfGrouded();
        onSlope = OnSlope();

        GetInput();
        UpdateVelocity();

        Crouch();
        
        Move();
    }



    /*private void StateHandler()
    {
        if (isGrounded && controls.Player.Walk.IsPressed())
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        else if (isGrounded && controls.Player.Crouch.IsPressed())
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        else if (controls.Player.Crouch.WasReleasedThisFrame())
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            playerHeight = startYScale;
        }

        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
            moveSpeed = walkSpeed;
        }
    }*/

    private void StateHandlerr()
    {
        if (!isGrounded)
        {
            state = MovementState.air;
        }
        else
        {
            if (controls.Player.Crouch.IsPressed())
            {
                if (false)
                    state = MovementState.sliding;
                else
                    state = MovementState.crouching;
            }
            else if (controls.Player.Walk.IsPressed())
            {
                state = MovementState.walking;
            }
            else
            {
                state = MovementState.sprinting;
            }
        }
    }


    // -------------- Movement -------------- \\


    private void GetInput()
    {
        inputVector = controls.Player.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        if (onSlope && !exitingSlope)
            characterCont.Move(GetSlopeMoveDir() * moveSpeed * Time.deltaTime);
        else
            characterCont.Move(
                moveDirection.normalized * (state == MovementState.walking ? walkSpeed : (state == MovementState.sprinting ? sprintSpeed : crouchSpeed)) * Time.deltaTime); // ohhhhh hell no
    }

    private void Crouch() 
    {
        if (controls.Player.Crouch.WasReleasedThisFrame() || state == MovementState.air)
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        if (state != MovementState.crouching)
            return;

        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        playerHeight = crouchYScale;
    }

    // ~~~~~ Slope

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 1f, groundLM))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDir()
        => Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;


    // -------------- Jumping -------------- \\


    private void Jump(InputAction.CallbackContext context)
    {
        if (state == MovementState.air)
            return;

        /*if (canJump)
            canJump = false;

        else if (hasDoubleJump)
            hasDoubleJump = false;

        else return;*/

        exitingSlope = true;

        velocity.y += jumpForce;

        ResetJump();
    }

    private void UpdateVelocity()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else
            velocity.y += gravity * Time.deltaTime;

        characterCont.Move(velocity * Time.deltaTime);
    }

    private void ResetJump()
    {
        canJump = true;
        hasDoubleJump = true;
        exitingSlope = false;
    }

    private bool CheckIfGrouded()
        => Physics.CheckSphere(new Vector3(transform.position.x, groundCheck.position.y, transform.position.z), checkRadius, groundLM, QueryTriggerInteraction.Ignore);
}