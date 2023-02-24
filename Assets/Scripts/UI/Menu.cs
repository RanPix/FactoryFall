using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using PlayerSettings;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }
    public Look look;
    private PlayerControls controls;
    public bool canOpenMenu = true;
    private bool isOpened = false;
    private bool wasOpened = false;
    [SerializeField] private GameObject panel;
    [SerializeField] private OpenAndCloseSettings openAndCloseSettings;

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

        OpenOrCloseMenu(false);
    }
    private void OnDestroy()
    {
        controls.UI.OpenOrCloseMenu.performed -= OpenOrCloseMenu;

    }


    public void OpenOrCloseMenu(InputAction.CallbackContext context)
        => OpenOrCloseMenu(!isOpened);
    public void OpenOrCloseMenu(bool openMenu)
    {
        if (openAndCloseSettings.isOpened)
        {
            openAndCloseSettings.CloseSettings();
            return;
        }
        WeaponKeyCodes _localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<WeaponKeyCodes>();
        CursorManager.instance.disablesToLockCount = openMenu ? CursorManager.instance.disablesToLockCount +1 : wasOpened ? CursorManager.instance.disablesToLockCount - 1: CursorManager.instance.disablesToLockCount;
        _localPlayer.weaponHolder.GetComponent<WeaponSway>().canSway = !openMenu;
        if(_localPlayer.currentWeapon)
            _localPlayer.currentWeapon.canShoot = !openMenu;

        isOpened = openMenu;
        look.canRotateCamera = !openMenu;
        if (!wasOpened && isOpened)
            wasOpened = true;
        CursorManager.instance.SetCursorLockState(openMenu ? CursorLockMode.None : CursorLockMode.Locked);
        panel.SetActive(openMenu);
    }
}
