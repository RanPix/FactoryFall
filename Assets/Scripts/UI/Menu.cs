using UnityEngine;
using UnityEngine.InputSystem;
using PlayerSettings;
using Mirror;

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

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    private void OnDestroy()
    {
        controls.UI.OpenOrCloseMenu.performed -= OpenOrCloseMenu;
    }

    public void Setup()
    {
        controls = new PlayerControls();
        controls.UI.Enable();
        controls.UI.OpenOrCloseMenu.performed += OpenOrCloseMenu;


        OpenOrCloseMenu(false);
    }

    public void Disconnect()
    {
        NetworkIdentity player = NetworkClient.localPlayer.GetComponent<NetworkIdentity>();

        if (player.isClient && !player.isServer)
        {
            NetworkManager.singleton.StopClient();
        }
        else if (player.isClient && player.isServer)
        {
            NetworkManager.singleton.StopHost();
        }
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

        WeaponKeyCodes _localPlayer = NetworkClient.localPlayer.GetComponent<WeaponKeyCodes>();
        //print($"is open = {openMenu}");
        CursorManager.instance.disablesToLockCount = openMenu ? CursorManager.instance.disablesToLockCount + 1 : wasOpened ? CursorManager.instance.disablesToLockCount - 1: CursorManager.instance.disablesToLockCount;
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
