using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerSettings;

public class Look : MonoBehaviour
{
    const string nickNameLayer = "Nickname";

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

    private bool lookAtPlayer = true;

    private Transform spectatePlayer;



    private void Start()
    {
        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.FreeCursor.performed += ControlCursor;
        UpdateFOV();
    }
    private void OnDestroy()
    {
        controls.Player.FreeCursor.performed -= ControlCursor;
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

    public void UpdateFOV()
    {
        cam.fieldOfView = Settings.FOV;
    }

    public void UpdateShowingNickNames()
    {
        bool isShowingNickNames = Settings.isShowingNickNames;
        // if (isShowingNickNames)
            //cam.cullingMask |= (1 << layerToAdd);//PLS END THIS
    }


    private void UpdateCamera()
    {
        if(!cam || !orientation || !canRotateCamera)
            return;

        if (!lookAtPlayer && spectatePlayer)
        {
            cam.transform.LookAt(spectatePlayer.position + new Vector3(0, 1f));

            return;
        }

        // Laggy beauty
        yRot += inputVector.x * 0.01f * Settings.sensetivity;
        xRot -= inputVector.y * 0.01f * Settings.sensetivity;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }

    public void SetRotation(float x, float y)
    {
        xRot = x;
        yRot = y;
    }


    public void SetupEvents(GamePlayer player)
    {
        player.OnDeath += DisableLook;
        player.OnRespawn += EnableLook;

    }

    private void DisableLook(string netID, Team team, string name, int hp)
    {
        spectatePlayer = GameManager.GetPlayer(netID)?.transform;
        lookAtPlayer = false;
    }
    private void EnableLook() => lookAtPlayer = true;    


    private void ControlCursor(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            CursorManager.instance.SetCursorLockState(CursorLockMode.None);
        else if (Cursor.lockState == CursorLockMode.None)
            CursorManager.instance.SetCursorLockState(CursorLockMode.Locked);
    }
}

