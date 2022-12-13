using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Look : NetworkBehaviour
{
    private PlayerControls controls;

    [HideInInspector] public InventoryUI inventoryUI;

    [SerializeField] private Camera m_Camera;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    
    private const float smoothing = 0.1f;

    [HideInInspector] public Transform orientation;
    [HideInInspector] public bool _isLocalPlayer { get; set; } = false;
    


    private Vector2 inputVector;
    private float xRot;
    private float yRot;

    private void Start()
    {
        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.FreeCursor.performed += ControlCursor;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(!_isLocalPlayer)
            return;

        GetInput();
        UpdateCamera();
    }

    private void GetInput()
        => inputVector = controls.Player.Look.ReadValue<Vector2>();

   
    private void UpdateCamera()
    {
        bool isInventoryOpened = inventoryUI.isPanelOpened;
        Cursor.lockState = isInventoryOpened ?
            CursorLockMode.Confined : 
            CursorLockMode.Locked;
        Cursor.visible = isInventoryOpened;
        if (!isInventoryOpened)
        {
            yRot += inputVector.x * 0.01f * sensX;
            xRot -= inputVector.y * 0.01f * sensY;
        }
        // Laggy beauty
        //yRot = Mathf.LerpAngle(yRot, yRot + inputVector.x, smoothing);
        //xRot = Mathf.LerpAngle(xRot, xRot - inputVector.y, smoothing);
        
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        m_Camera.transform.rotation = Quaternion.Euler(xRot, yRot, 0);
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
