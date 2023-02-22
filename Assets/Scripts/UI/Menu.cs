using System.Collections;
using System.Collections.Generic;
using Player;
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
    private bool wasOpened = false;
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

        OpenOrCloseMenu(false);
    }

    public void OpenOrCloseMenu(InputAction.CallbackContext context)
    {
        WeaponKeyCodes _localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<WeaponKeyCodes>();
        CursorManager.instance.disablesToLockCount = isOpened ? CursorManager.instance.disablesToLockCount - 1 : CursorManager.instance.disablesToLockCount + 1;
        _localPlayer.weaponHolder.GetComponent<WeaponSway>().canSway = isOpened;

        if(_localPlayer.currentWeapon)
            _localPlayer.currentWeapon.canShoot = isOpened;

        isOpened = !isOpened;
        look.canRotateCamera = !isOpened;

        CursorManager.instance.SetCursorLockState(isOpened ? CursorLockMode.None : CursorLockMode.Locked);
        panel.SetActive(isOpened);
    }
    public void OpenOrCloseMenu(bool openMenu)
    {
        WeaponKeyCodes _localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<WeaponKeyCodes>();
        CursorManager.instance.disablesToLockCount = openMenu ? CursorManager.instance.disablesToLockCount +1 : wasOpened ? CursorManager.instance.disablesToLockCount -1: CursorManager.instance.disablesToLockCount;
        _localPlayer.weaponHolder.GetComponent<WeaponSway>().canSway = !openMenu;
        if(_localPlayer.currentWeapon)
            _localPlayer.currentWeapon.canShoot = !openMenu;

        isOpened = openMenu;
        look.canRotateCamera = !openMenu;
        CursorManager.instance.SetCursorLockState(openMenu ? CursorLockMode.None : CursorLockMode.Locked);
        panel.SetActive(openMenu);
    }
}
