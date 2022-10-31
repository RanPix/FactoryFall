using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Required")]

    private Rigidbody rigidBody;
    private PlayerControls controls;
    

    [Header("Move")]

    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;

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

        
    }

    private void FixedUpdate()
    {
        DragControl();

        GetInput();

        Crouch();
        Move();
        SpeedControl();

        StateHandler();

        //print(rigidBody.velocity);
    }

    private void GetInput()
    {
        inputVector = controls.Player.Move.ReadValue<Vector2>();
    }

    private void StateHandler()
    {
        if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else if (isGrounded && controls.Player.Sprint.IsPressed())
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        else if (controls.Player.Crouch.IsPressed())
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        else if (controls.Player.Crouch.WasReleasedThisFrame())
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        else
        {
            state = MovementState.air;
        }
    }

    private void Move()
    {
        moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        if (isGrounded)
            rigidBody.AddForce(moveDirection.normalized * 10 * moveSpeed, ForceMode.Force);
        else
            rigidBody.AddForce(moveDirection.normalized * 10 * moveSpeed * airMultiplier, ForceMode.Force);
    }

    private void Crouch()
    {
        if (!controls.Player.Crouch.IsPressed())
            return;

        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        
        if (isGrounded && controls.Player.Crouch.WasPressedThisFrame())
            rigidBody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
        }
    }

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

        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        ResetJump();
    }

    private void ResetJump()
    {
        canJump = true;
        hasDoubleJump = true;
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
