using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public Look look;
    private PlayerControls controls;
    private bool isOpened = false;
    [SerializeField] private GameObject panel;

    void Start()
    {
        controls = new PlayerControls();
        controls.UI.Enable();
        controls.UI.OpenOrCloseMenu.performed += OpenOrCloseMenu;

        CloseMenu();
    }

    public void OpenOrCloseMenu(InputAction.CallbackContext context)
    {
        isOpened = !isOpened;
        look.isMenuOpened = isOpened;
        Cursor.visible = isOpened;
        Cursor.lockState = isOpened? CursorLockMode.None : CursorLockMode.Locked;
        panel.SetActive(isOpened);
    }

    public void CloseMenu()
    {
        isOpened = false;
        look.isMenuOpened = isOpened;
        Cursor.visible = isOpened;
        Cursor.lockState = isOpened ? CursorLockMode.None : CursorLockMode.Locked;
        panel.SetActive(isOpened);
    }
}
