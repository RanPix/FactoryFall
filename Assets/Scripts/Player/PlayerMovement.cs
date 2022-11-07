using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Mirror;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("Required")]

    private CharacterController characterCont;
    private PlayerControls controls;

    [SerializeField] private float gravity;
    private Vector3 velocity;

    [Header("Camera")]

    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private Transform cameraPosition;

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

    [Space]

    [SerializeField] private float speedSlideIncrease;
    private bool gotSlideSpeedIncrease;

    [Header("Slope")]

    [SerializeField] private float maxSlopeAngle;
    private float playerHeight;
    private RaycastHit slopeHit;

    private bool onSlope;
    private bool exitingSlope;


    [Header("Jump")]

    [SerializeField] private float jumpForce;

    private bool isGrounded;

    [Space]

    [SerializeField] private int maxDoubleJumps = 2;
    private int hasDoubleJumps;


    [Header("Wallrun")]




    [Header("Redirect")]

    [SerializeField] private int maxRedirects = 2;
    private int hasRedirects;


    [Header("Grapple")]

    [Header("Required")]

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask groundLM;
    [SerializeField] private LayerMask wallLM;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float ceilingCheckRadius;



    private MovementState state;


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

        // Old Awake

        cameraHolder = Instantiate(cameraHolder);
        cameraHolder.GetComponent<MoveCamera>().cameraPosition = cameraPosition;
        cameraHolder.GetComponentInChildren<Look>().orientation = orientation;

        characterCont = GetComponent<CharacterController>();

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Jump.performed += Jump;
        controls.Player.Redirect.performed += Redirect;

        // True Start

        startYScale = transform.localScale.y;
        playerHeight = startYScale;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;


        isGrounded = CheckIfGrouded();
        onSlope = IsOnSlope();

        StateHandler();

        GetInput();
        UpdateVelocity();

        print($"{moveDirection.magnitude}, {velocity.magnitude}, {inputVector}, {isGrounded}");
        print(HasCeiling());

        if (state == MovementState.crouching | state == MovementState.sliding)
            Crouch();
        else
            Uncrouch();

        if(state == MovementState.sliding)
            Slide();
        else if (isGrounded)
            Move();
        else
            AirMove();
    }

    private void LateUpdate()
    {
        if(isGrounded)
            ResetAgilityAbilities();
    }

    private void StateHandler()
    {
        if (controls.Player.Crouch.IsPressed())
        {
            if (moveDirection.magnitude >= sprintSpeed)
                state = MovementState.sliding;
            else
                state = MovementState.crouching;
        }
        else if (state == MovementState.crouching && HasCeiling())
            return;
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
        moveDirection = (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized * 
            (state == MovementState.crouching ? crouchSpeed : (state == MovementState.sprinting ? sprintSpeed : walkSpeed)); // Speed selection

        characterCont.Move(moveDirection * Time.deltaTime);
    }

    private void AirMove()
    {
        moveDirection = (moveDirection + (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized * airMultiplier).normalized * moveDirection.magnitude;
        characterCont.Move(moveDirection * Time.deltaTime);
    }

    private void Crouch() 
        => transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);

    private void Slide()
    {
        if (!gotSlideSpeedIncrease)
        {
            moveDirection *= speedSlideIncrease;
            gotSlideSpeedIncrease = true;
        }

        moveDirection = Vector3.Lerp()

        characterCont.Move(moveDirection * Time.deltaTime)
    }

    private void Uncrouch()
        => transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

    // ~~~~~ Slope

    private Vector3 GetSlopeMoveDir()
        => Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;


    // -------------- Jumping -------------- \\


    private void Jump(InputAction.CallbackContext context)
    {
        if (!isGrounded && hasDoubleJumps-- < 1)
            return;

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
        hasDoubleJumps = maxDoubleJumps;
        hasRedirects = maxRedirects;
        gotSlideSpeedIncrease = false;
    }

    // Bool checks

    private bool CheckIfGrouded()
        => Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLM, QueryTriggerInteraction.Ignore);

    private bool HasCeiling()
        => Physics.CheckSphere(ceilingCheck.position, ceilingCheckRadius, groundLM, QueryTriggerInteraction.Ignore);

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