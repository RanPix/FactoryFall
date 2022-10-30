using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Required")]

    private Rigidbody rigidBody;
    private PlayerControls controls;
    

    [Header("Move")]

    [SerializeField] private float moveSpeed;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float maxSpeed;

    private Vector2 inputVector;
    private Vector3 moveDirection;

    [Space]
    [SerializeField] private Transform orientation;

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


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Jump.performed += Jump;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        isGrounded = CheckIfGrouded();

        
    }

    private void FixedUpdate()
    {
        DragControl();

        GetInput();
        Move();
        SpeedControl();

        //print(rigidBody.velocity);
    }

    private void GetInput()
        => inputVector = controls.Player.Move.ReadValue<Vector2>();

    private void Move()
    {
        moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        if (isGrounded)
            rigidBody.AddForce(moveDirection.normalized * 10 * moveSpeed, ForceMode.Force);
        else
            rigidBody.AddForce(moveDirection.normalized * 10 * moveSpeed * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        if(flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        print(true);

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
