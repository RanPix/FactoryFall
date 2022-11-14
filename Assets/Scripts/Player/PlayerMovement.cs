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


    private Camera camera;

    [Header("Move")]

    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float accelerateLerpTime;

    [Space]

    [SerializeField, Range(0, 1f)] private float airMultiplier = 0.2f;
    [SerializeField, Range(0, 1f)] private float maxAirStrafeSpeed = 25;

    private Vector2 inputVector;
    private Vector3 moveDirection;

    [Space]
    


    [Header("Crouching")]

    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchYScale;
    private float startYScale;

    [Space]

    [SerializeField] private float speedSlideIncrease = 1.5f;
    [SerializeField] private float slideSpeedDeaccelerate = 1f;
    [SerializeField] private float slideStrafeMultiplier = 0.1f;
    [SerializeField] private float maxSlideboosSpeed = 25;
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

    [Header("Fov")]

    [SerializeField] private float fovTime = 0.25f;

    [Space]

    [SerializeField] private float normalFov = 85f;
    [SerializeField] private float redirectFov = 100f;
    [SerializeField] private float slideFov = 100f;

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
        camera = Camera.main;

        characterCont = GetComponent<CharacterController>();

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Jump.performed += Jump;
        controls.Player.Redirect.performed += Redirect;

        // True Start

        startYScale = transform.localScale.y;
        playerHeight = startYScale;
    }

    [Client]
    private void Update()
    {
        if (!isLocalPlayer)
            return;


        isGrounded = CheckIfGrouded();
        onSlope = IsOnSlope();

        StateHandler();

        GetInput();
        UpdateVelocity();

        //print($"{moveDirection.magnitude}, {velocity.magnitude}, {inputVector}, {isGrounded}");
        //print(HasCeiling());

        if (state == MovementState.crouching | state == MovementState.sliding)
            Crouch();
        else
            Uncrouch();


        if (isGrounded)
            if (state == MovementState.sliding)
                Slide();
            else
            {
                Move();
            }
        else
        {
            AirMove();
        }
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer)
            return;
        if (isGrounded)
            ResetAgilityAbilities();
        if (state != MovementState.sliding)
            gotSlideSpeedIncrease = false;
    }

    private void StateHandler()
    {
        if (controls.Player.Crouch.IsPressed())
        {
            if (moveDirection.magnitude >= sprintSpeed*0.75 || (state == MovementState.sliding && moveDirection.magnitude > crouchSpeed))
                state = MovementState.sliding;
            else
                state = MovementState.crouching;
        }
        else if ((state == MovementState.crouching || state == MovementState.sliding) && HasCeiling())
            return;
        else if (controls.Player.Sprint.IsPressed() & ChechIfForward())
        {
            state = MovementState.sprinting;
        }
        else
        {
            state = MovementState.walking;
        }
    }


    #region OldMovement


    private void GetInput()
        => inputVector = controls.Player.Move.ReadValue<Vector2>();

    private void Move()
    {
        moveDirection = Vector3.Lerp(moveDirection, (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized *
            (state == MovementState.crouching ? crouchSpeed : (state == MovementState.sprinting ? sprintSpeed : walkSpeed)), accelerateLerpTime * Time.deltaTime); // Speed selection

        characterCont.Move(moveDirection * Time.deltaTime);
    }

    private void AirMove()
    {
        var moveDirectionMagnitude = moveDirection.magnitude;
        var orientationXZ = orientation.forward * inputVector.y + orientation.right * inputVector.x;
        var orientationXZNormalized = orientationXZ.normalized;

        print($"print Before: moveDirection: {moveDirection}, {orientationXZNormalized}, {airMultiplier}, {maxAirStrafeSpeed}");

        if (moveDirectionMagnitude < maxAirStrafeSpeed)
        {
            moveDirection += orientationXZNormalized * airMultiplier;
        }
        else
        {
            moveDirection =
                (moveDirection + orientationXZNormalized *
                    airMultiplier).normalized * moveDirectionMagnitude;
        }

        print($"print After: moveDirection: {moveDirection}, {orientationXZNormalized}, {airMultiplier}, {maxAirStrafeSpeed}");

        characterCont.Move(moveDirection * Time.deltaTime);
    }

    private void Crouch() 
        => transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);

    private void Slide()
    {
        if (!gotSlideSpeedIncrease && moveDirection.magnitude < maxSlideboosSpeed)
        {
            moveDirection *= speedSlideIncrease;
            gotSlideSpeedIncrease = true;
        }

        moveDirection *= (moveDirection.magnitude - slideSpeedDeaccelerate) / moveDirection.magnitude;
        moveDirection = (moveDirection + (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized * slideStrafeMultiplier).normalized * moveDirection.magnitude;


        characterCont.Move(moveDirection * Time.deltaTime);
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

        velocity.y = velocity.y < jumpForce ? jumpForce : velocity.y + jumpForce;
    }

    private void Redirect(InputAction.CallbackContext context)
    {
        if (hasRedirects-- > 0)
        {
            moveDirection = (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized * moveDirection.magnitude;
        }
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

        if (angle < 46 || angle > 315)
            return true;

        return false;
    }

    #endregion

    #region New Movement



    #endregion

    private void OnDestroy()
    {
        //Destroy(cameraHolder);
    }
}