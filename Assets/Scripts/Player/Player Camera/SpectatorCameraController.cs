using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEditor.Rendering.LookDev;
using System.Xml.Serialization;

public enum Pan
{
    X,
    Y, 
    Z,
    NULL,
}


public class SpectatorCameraController : MonoBehaviour
{
    private PlayerControls controls;


    [SerializeField] private Camera spectateCamera;
    private Camera mainCamera;

    [SerializeField] private Look look;

    [SerializeField] private float speed;
    [SerializeField] private float speedChange;
    [SerializeField] private float smoothness;

    private bool isInSpectate = false;
    public Action<bool> OnCameraChange;

    private Vector3 wayPoint;

    [SerializeField] Transform randomTransformForOrientationCalledSIIIUUUU;

    private void Start()
    {
        controls = new PlayerControls();

        controls.SpectateCamera.Enable();
        controls.SpectateCamera.Pan.performed += PanView;
        controls.SpectateCamera.ToggleSpectator.performed += YesChangeCamera;
        controls.SpectateCamera.ChangeSpeed.performed += ChangeSpeed;

        look._isLocalPlayer = true;

        look.orientation = randomTransformForOrientationCalledSIIIUUUU;
        look.canRotateCamera = true;

        spectateCamera.enabled = false;
        spectateCamera.GetComponent<AudioListener>().enabled = false;

        wayPoint = transform.position;
    }

    private void OnDestroy()
    {
        controls.SpectateCamera.Pan.performed -= PanView;
        controls.SpectateCamera.ToggleSpectator.performed -= YesChangeCamera;
        controls.SpectateCamera.ChangeSpeed.performed -= ChangeSpeed;
    }
    public void Setup()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!isInSpectate)
            return;

        Vector3 moveVec = controls.SpectateCamera.Move.ReadValue<Vector3>();
        Transform currentCamTransform = spectateCamera.transform;

        Vector3 pos = transform.position;

        Vector3 input = currentCamTransform.forward * moveVec.z + currentCamTransform.right * moveVec.x + currentCamTransform.up * moveVec.y;

        wayPoint = wayPoint + input * speed * Time.deltaTime;
        pos = Vector3.Lerp(pos, wayPoint, smoothness * Time.deltaTime);

        transform.position = pos;
    }

    private void ChangeSpeed(InputAction.CallbackContext context)
    {
        int value = context.ReadValue<float>() < 0 ? -1 : 1;

        speed += value < 0 ? -speedChange : speedChange;
    }

    private void PanView(InputAction.CallbackContext context)
    {
        int _panValue = (int)context.ReadValue<float>();

        Quaternion newCamRotation = spectateCamera.transform.rotation;

        switch (_panValue)
        {
            case 0: // X

                int _y = (int)(newCamRotation.eulerAngles.y - newCamRotation.eulerAngles.y % 90);

                _y = _y == 270 ? 90 : 270;
                look.SetRotation(0f, _y);

                newCamRotation.eulerAngles = Quaternion.Euler(0f, _y, 0f).eulerAngles;

                break;

            case 1: // Y
                int _x = (int)(newCamRotation.eulerAngles.x - newCamRotation.eulerAngles.x % 90);


                _x = _x == 90 ? -90 : 90;
                look.SetRotation(_x, 0f);

                newCamRotation.eulerAngles = Quaternion.Euler(_x, 0f, 0f).eulerAngles;
                break;

            case 2: // Z
                int _z = (int)(newCamRotation.eulerAngles.y - newCamRotation.eulerAngles.y % 90);

                _z = _z == 180 ? 0 : 180;
                look.SetRotation(0f, _z);

                newCamRotation.eulerAngles = Quaternion.Euler(0f, _z, 0f).eulerAngles;


                break;

        }

        
        spectateCamera.transform.rotation = newCamRotation;
        
    }
    private void YesChangeCamera(InputAction.CallbackContext context)
        => ChangeCamera();

    public void ChangeCamera()
    {
        look._isLocalPlayer = true;

        look.orientation = randomTransformForOrientationCalledSIIIUUUU;
        look.canRotateCamera = true;

        isInSpectate = !isInSpectate; 

        if (isInSpectate)
        {
            CursorManager.instance.SetLockStateImmediately(CursorLockMode.Locked);
        }

        mainCamera.enabled = !isInSpectate;
        mainCamera.GetComponent<AudioListener>().enabled = !isInSpectate;
        mainCamera.GetComponent<Camera>().enabled = !isInSpectate;

        spectateCamera.enabled  = isInSpectate;
        spectateCamera.GetComponent<AudioListener>().enabled = isInSpectate;

        look.enabled = isInSpectate;

        OnCameraChange?.Invoke(!isInSpectate);
    }
}
