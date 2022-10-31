using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementt : MonoBehaviour
{
    [Header("Required")]

    private Rigidbody rigidBody;
    private PlayerControls controls;


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

    [Header("Slope Handling")]

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

    [Header("Drag")]

    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;

    [SerializeField] private MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Jump.performed += Jump;
    }

    private void Start()
    {
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        isGrounded = CheckIfGrouded();
        onSlope = OnSlope();

        Crouch();
        StateHandler();
    }

    private void FixedUpdate()
    {
        DragControl();

        GetInput();

        Move();
        SpeedControl();
    }


    private void StateHandler()
    {
        if (isGrounded && controls.Player.Sprint.IsPressed())
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
    }


    // -------------- Movement --------------


    private void GetInput()
    {
        inputVector = controls.Player.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        if (onSlope && !exitingSlope)
        {
            print(moveSpeed);
            rigidBody.AddForce(GetSlopeMoveDir() * moveSpeed * 15f, ForceMode.Force);

            if (rigidBody.velocity.y > 0)
                rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        else if (isGrounded)
            rigidBody.AddForce(moveDirection.normalized * 10f * moveSpeed, ForceMode.Force);
        else
            rigidBody.AddForce(moveDirection.normalized * 10f * moveSpeed * airMultiplier, ForceMode.Force);

        rigidBody.useGravity = !onSlope;
    }

    private void Crouch()
    {
        if (!controls.Player.Crouch.IsPressed())
            return;

        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        playerHeight = crouchYScale;

        if (isGrounded && controls.Player.Crouch.WasPressedThisFrame())
        {
            rigidBody.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        }
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

    // ~~~~~ Speed

    private void SpeedControl()
    {
        if (onSlope && !exitingSlope)
        {
            if (rigidBody.velocity.magnitude > moveSpeed)
                rigidBody.velocity = rigidBody.velocity.normalized * moveSpeed;
        }

        else
        {
            Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
            }
        }

    }


    // -------------- Jumping --------------


    private void Jump(InputAction.CallbackContext context)
    {
        isGrounded = CheckIfGrouded();
        if (!isGrounded)
            return;

        /*if (canJump)
            canJump = false;

        else if (hasDoubleJump)
            hasDoubleJump = false;

        else return;*/

        exitingSlope = true;

        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        ResetJump();
    }

    private void ResetJump()
    {
        canJump = true;
        hasDoubleJump = true;
        exitingSlope = false;
    }

    private bool CheckIfGrouded()
        => Physics.CheckSphere(new Vector3(transform.position.x, groundCheck.position.y, transform.position.z), checkRadius, groundLM, QueryTriggerInteraction.Ignore);

    private void DragControl()
    {
        if (isGrounded)
            rigidBody.drag = groundDrag;
        else
            rigidBody.drag = airDrag;
    }
}
