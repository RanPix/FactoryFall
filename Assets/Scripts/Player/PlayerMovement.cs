using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("Required")]

    private CharacterController characterCont;
    private PlayerControls controls;

    [SerializeField] private float gravity;
    private Vector3 velocity;

    [Header("Camera")]

    [SerializeField] private Transform cameraPos;

    [Space]
    [SerializeField] private GameObject cameraPrefab;

    [Header("Move")]

    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float maxSpeed; 
    private float moveSpeed;

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

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        cameraPrefab = Instantiate(cameraPrefab);
        cameraPrefab.GetComponent<MoveCamera>().cameraPosition = cameraPos;
        cameraPrefab.GetComponentInChildren<Look>().orientation = orientation;

        characterCont = GetComponent<CharacterController>();

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Jump.performed += Jump;
        controls.Player.Redirect.performed += Redirect;
    }

        startYScale = transform.localScale.y;
        playerHeight = startYScale;
    }

    [Client]
    private void Update()
    {
        
        if (!isLocalPlayer)
            return;

        StateHandlerr();
        isGrounded = CheckIfGrouded();
        onSlope = OnSlope();

        GetInput();
        UpdateVelocity();

        Crouch();
        
        Move();
        AirMove();
    }

    private void LateUpdate()
    {
        ResetJump();
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
    {
        inputVector = controls.Player.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        if (!isGrounded)
            return;

        moveDirection = (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized * (state == MovementState.crouching ? crouchSpeed : (state == MovementState.sprinting ? sprintSpeed : walkSpeed));

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
        if (!isGrounded && !hasDoubleJump)
            return;
        if (hasDoubleJump)
            hasDoubleJump = false;

        exitingSlope = true;

        velocity.y += jumpForce;
    }

    private void Redirect(InputAction.CallbackContext context)
        => moveDirection = (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized * moveDirection.magnitude;

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
        if (!isGrounded)
            return;

        hasDoubleJump = true;
        exitingSlope = false;
    }

    // Bool checks

    private bool CheckIfGrouded()
        => Physics.CheckSphere(new Vector3(transform.position.x, groundCheck.position.y, transform.position.z), checkRadius, groundLM, QueryTriggerInteraction.Ignore);

    private bool ChechIfForward()
    {
        float angle = Quaternion.LookRotation(moveDirection).eulerAngles.y - orientation.eulerAngles.y;
        angle = angle < 0 ? -angle : angle; // Handmade Abs)

        if (angle < 46 || angle == 315)
            return true;

        return false;
    }

    private void OnDestroy()
    {
        Destroy(cameraPrefab);
    }
}