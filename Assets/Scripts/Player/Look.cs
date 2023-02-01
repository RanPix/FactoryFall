using GameBase;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    private PlayerControls controls;

    [HideInInspector] public bool canRotateCamera;

    [SerializeField] private Camera cam;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    private const float smoothing = 0.1f;

    [HideInInspector] public Transform orientation;
    [HideInInspector] public bool _isLocalPlayer { get; set; } = false;


    private Vector2 inputVector;
    private float xRot;
    private float yRot;

    private bool lookEnabled = true;

    private Transform spectatePlayer;



    private void Start()
    {
        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.FreeCursor.performed += ControlCursor;
    }

    private void Update()
    {
        if (!_isLocalPlayer)
            return;

        GetInput();
        UpdateCamera();
    }

    private void GetInput()
        => inputVector = controls.Player.Look.ReadValue<Vector2>();


    private void UpdateCamera()
    {
        if (!lookEnabled)
        {
            cam.transform.LookAt(spectatePlayer.position + new Vector3(0, 1f));

            return;
        }

        if (!canRotateCamera)
            return;

        // Laggy beauty
        yRot += inputVector.x * 0.01f * sensX;
        xRot -= inputVector.y * 0.01f * sensY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }


    public void SetupEvents(GamePlayer player)
    {
        player.OnDeath += DisableLook;
        player.OnRespawn += EnableLook;

    }

    private void DisableLook(string netID, Team team, string name, int hp)
    {
        spectatePlayer = GameManager.GetPlayer(netID)?.transform;
        lookEnabled = false;
    }
    private void EnableLook() => lookEnabled = true;    


    private void ControlCursor(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            CursorManager.SetCursorLockState(CursorLockMode.None);
        else if (Cursor.lockState == CursorLockMode.None)
            CursorManager.SetCursorLockState(CursorLockMode.Locked);
    }
}

