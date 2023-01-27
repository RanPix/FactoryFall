using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }
    public Look look;
    private PlayerControls controls;
    public bool canOpenMenu = true;
    private bool isOpened = false;
    [SerializeField] private GameObject panel;

    void Awake()
    {
        if(Instance==null)
            Instance = this;
    }
    void Start()
    {
        controls = new PlayerControls();
        controls.UI.Enable();
        controls.UI.OpenOrCloseMenu.performed += OpenOrCloseMenu;

        CloseMenu();
    }

    public void OpenOrCloseMenu(InputAction.CallbackContext context)
    {
        CursorManager.canLock = isOpened ? false : true;
        isOpened = !isOpened;
        look.canRotateCamera = !isOpened;
        CursorManager.SetCursorLockState(isOpened ? CursorLockMode.None : CursorLockMode.Locked);
        panel.SetActive(isOpened);
    }

    public void CloseMenu()
    {
        CursorManager.canLock = true;
        isOpened = false;
        look.canRotateCamera = !isOpened;
        CursorManager.SetCursorLockState(CursorLockMode.None);
        panel.SetActive(isOpened);
    }
}
