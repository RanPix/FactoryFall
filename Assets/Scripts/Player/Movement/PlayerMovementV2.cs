using Mirror;
using UnityEngine;
using FiniteStateMachine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource), typeof(Animator))]
public class PlayerMovementV2 : StateMachine
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

    public CharacterController characterCont { get; private set; }
    public PlayerControls controls { get; private set; }
    private Camera camera;
    private Animator animator;

    [Header("Speed")]

    [SerializeField] private float sprintSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float crouchSpeed;
    [Space]
    [SerializeField] private float accelerationLerp;

    [field: SerializeField, Range(0f, 1f)] public float airStrafeMultiplier { get; private set; }
    [field: SerializeField, Range(0f, 1f)] public float slideStrafeMultiplier { get; private set; }

    [Header("Jump")]

    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [Space]
    [SerializeField] private int maxDoubleJumps;
    private int hasDoubleJumps;

    [field: Header("Air Strafe")]
    [field: SerializeField] public float maxAirStrafeSpeed { get; private set; }


    private MovementState state;


    public enum MovementState
    {
        walking,
        sprinting,
        crouch
    }

    [HideInInspector] public BaseState idle { get; private set; }
    [HideInInspector] public BaseState walk { get; private set; }

    public Vector3 horizontalMoveDirection;
    public Vector3 verticalMoveDirection;
    private Vector3 input;

    private float desiredSpeed;
    private float magnitude;

    private int isCrouchedHash;

    private bool isOnGround;
    private bool hasCeiling;

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        camera = Camera.main;

        animator = GetComponent<Animator>();
        characterCont = GetComponent<CharacterController>();

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Jump.performed += Jump;

        isCrouchedHash = Animator.StringToHash("isCrouched");

        airStrafeMultiplier *= 20;
        slideStrafeMultiplier *= 20;

        idle = new Idle(this);
        walk = new Walk(this);

    }

    [Client]
    private void StateHandler()
    {
        if (controls.Player.Crouch.IsPressed())
        {
            state = MovementState.crouch;
        }
        else if (controls.Player.Sprint.IsPressed() && CheckIfMovingForward())
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

    protected override BaseState GetInitialState()
    {
        return idle;
    }



    #region Move

    

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


    #region Jump

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

    public bool CheckIfOnGround()
        => Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLM, QueryTriggerInteraction.Ignore);

    public bool CheckIfHasCeiling()
        => Physics.CheckSphere(ceilingCheck.position, ceilingCheckRadius, groundLM, QueryTriggerInteraction.Ignore);

    public bool CheckIfMovingForward()
    {
        float angle = Quaternion.LookRotation(horizontalMoveDirection).eulerAngles.y - orientation.eulerAngles.y;
        angle = angle < 0 ? -angle : angle; // Handmade Abs)

        return angle is < 50 or > 310;
    }

    #endregion
}
