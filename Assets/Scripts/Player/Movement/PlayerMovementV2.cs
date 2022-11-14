using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class PlayerMovementV2 : NetworkBehaviour
{
    [Header("Required")]

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [Space]
    [SerializeField] private LayerMask groundLM;
    [SerializeField] private LayerMask wallLM;
    [Space]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float ceilingCheckRadius;

    private CharacterController characterCont;
    private PlayerControls controls;
    private Camera camera;

    [Header("Speed")]

    [SerializeField] private float sprintSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float crouchSpeed;
    [Space]
    [SerializeField] private float accelerationLerp;
    [SerializeField, Range(0f, 1f)] private float airStrafeMultiplier;
    [SerializeField, Range(0f, 1f)] private float slideStrafeMultiplier;

    [Header("Jump")]

    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [Space]
    [SerializeField] private int maxDoubleJumps;
    private int hasDoubleJumps;

    [Header("Air Strafe")]
    [SerializeField] private float maxAirStrafeSpeed;


    private MovementState state;


    public enum MovementState
    {
        walking,
        sprinting,

    }

    private Vector3 horizontalMoveDirection;
    private Vector3 verticalMoveDirection;
    private Vector3 input;

    private float desiredSpeed;
    private float magnitude;

    private bool isOnGround;
    private bool hasCeiling;

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        camera = Camera.main;

        characterCont = GetComponent<CharacterController>();

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Jump.performed += Jump;

        airStrafeMultiplier *= 20;
        slideStrafeMultiplier *= 20;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        isOnGround = CheckIfOnGround();
        hasCeiling = CheckIfHasCeiling();
        magnitude = horizontalMoveDirection.magnitude;

        GetInputs();
        StateHandler();

        Gravity();

        if (isOnGround)
        {
            GroundMovement();
            ResetDoubleJumps();
        }
        else
        {
            AirMovement();
        }

        Move();

        print($"{horizontalMoveDirection.magnitude}, {airStrafeMultiplier}");
    }

    [Client]
    private void StateHandler()
    {
        if (controls.Player.Sprint.IsPressed() && CheckIfMovingForward())
        {
            state = MovementState.sprinting;
            desiredSpeed = sprintSpeed;
        }
        else
        {
            state = MovementState.walking;
            desiredSpeed = walkSpeed;
        }
    }

    private void GetInputs()
    {
        Vector2 inputVector = controls.Player.Move.ReadValue<Vector2>();
        input = (orientation.forward * inputVector.y + orientation.right * inputVector.x).normalized;
    }

    #region Move

    private void Move()
        => characterCont.Move((horizontalMoveDirection + verticalMoveDirection) * Time.deltaTime);

    private void Gravity()
    {
        if (isOnGround && verticalMoveDirection.y < 0)
            verticalMoveDirection.y = 0;
        else
            verticalMoveDirection.y -= gravity * Time.deltaTime;


    }

    private void GroundMovement()
    {
        //float moveDirectionMagnitude = horizontalMoveDirection.magnitude;

        horizontalMoveDirection = Vector3.Lerp(horizontalMoveDirection, input * desiredSpeed, accelerationLerp * Time.deltaTime);
    }

    private void AirMovement()
    {
        if (magnitude < maxAirStrafeSpeed)
        {
            horizontalMoveDirection = horizontalMoveDirection + input * airStrafeMultiplier * Time.deltaTime;

            if (horizontalMoveDirection.magnitude > maxAirStrafeSpeed)
                horizontalMoveDirection = horizontalMoveDirection.normalized * maxAirStrafeSpeed;
        }
        else
        {
            horizontalMoveDirection = (horizontalMoveDirection + input * airStrafeMultiplier * Time.deltaTime).normalized * magnitude;    
        }
    }

    #region Jump

    private void Jump(InputAction.CallbackContext context)
    {
        if (!isOnGround && hasDoubleJumps-- < 1)
            return;

        if (verticalMoveDirection.y < jumpForce)
            verticalMoveDirection.y = jumpForce;
        else
            verticalMoveDirection.y += jumpForce;
    }

    private void ResetDoubleJumps()
        => hasDoubleJumps = maxDoubleJumps;

    private void ResetDoubleJumps(int jumpsToRecover)
    {
        if (hasDoubleJumps < 0)
            hasDoubleJumps = 0;

        if (hasDoubleJumps + jumpsToRecover < maxDoubleJumps)
            hasDoubleJumps += jumpsToRecover;
        else
            hasDoubleJumps = maxDoubleJumps;
    }

    #endregion

    #endregion

    #region Bool checks

    private bool CheckIfOnGround()
        => Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLM, QueryTriggerInteraction.Ignore);

    private bool CheckIfHasCeiling()
        => Physics.CheckSphere(ceilingCheck.position, ceilingCheckRadius, groundLM, QueryTriggerInteraction.Ignore);

    private bool CheckIfMovingForward()
    {
        float angle = Quaternion.LookRotation(horizontalMoveDirection).eulerAngles.y - orientation.eulerAngles.y;
        angle = angle < 0 ? -angle : angle; // Handmade Abs)

        if (angle < 50 || angle > 310)
            return true;

        return false;
    }

    #endregion
}
