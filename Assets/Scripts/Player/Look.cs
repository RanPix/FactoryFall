using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    private PlayerControls controls;

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    private const float smoothing = 0.1f;

    [HideInInspector] public Transform orientation;



    private Vector2 inputVector;
    private float xRot;
    private float yRot;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Enable();
    }

    private void Start()
    {
        controls.Player.FreeCursor.performed += ControlCursor;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetInput();
        UpdateCamera();
    }

    private void GetInput()
        => inputVector = controls.Player.Look.ReadValue<Vector2>();

   
    private void UpdateCamera()
    {
        yRot = Mathf.LerpAngle(yRot, yRot + inputVector.x, smoothing);
        xRot = Mathf.LerpAngle(xRot, xRot - inputVector.y, smoothing);

        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }

    private void ControlCursor(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;
        else if (Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;
    }
}
