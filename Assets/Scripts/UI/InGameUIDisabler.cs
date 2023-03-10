using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUIDisabler : MonoBehaviour
{
    private PlayerControls controls;

    [SerializeField] Menu menu;
    [SerializeField] Canvas canvas;

    public bool IsEnabled 
    { 
        get => _isEnabled;
        set 
        { 
            canvas.enabled = value;
            _isEnabled = value;
        }
    }

    private bool _isEnabled = true;


    private void ChangeIsEnabled(InputAction.CallbackContext context)
        => IsEnabled = !IsEnabled;

    private void OnDestroy()
    {
        controls.UI.DisableOrEnableUI.performed -= ChangeIsEnabled;
    }

    public void Setup()
    {
        controls = new PlayerControls();
        controls.UI.Enable();
        controls.UI.DisableOrEnableUI.performed += ChangeIsEnabled;
    }
}
