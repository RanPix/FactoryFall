using System.Collections.Generic;
using System.Linq;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

[RequireComponent(typeof(AudioSync))]
public class WeaponKeyCodes : NetworkBehaviour
{
    [SerializeField] private GamePlayer gamePlayer;
    [SerializeField] private MultiNetworkComponentsSorter multiNetworkComponentsSorter;
    [field: SerializeField] public Arm arm { get; private set; }

    [Space]
    [Header("Weapon")]
    public bool weaponsWasSelected = false;
    public Transform weaponHolder;
    public Weapon currentWeapon;
    public int currentWeaponIndex = -1;
    public GameObject[] weapons;
    public List<int> activatedWeaponsIndexes = new List<int>();
    [SyncVar] public int currentWeaponChildIndex = 0;

    [Space]
    [Header("Audio")]
    [SerializeField] private AudioClip changeWeaponClip;

    private PlayerControls controls;
    private AudioSync audioSync;
    private AudioSource weaponAudioSource;

    public bool canShoot = true;

    private void Start()
    {
        multiNetworkComponentsSorter = GetComponent<MultiNetworkComponentsSorter>();

        if(gamePlayer.team != Team.Null)
            arm.SetupArm(gamePlayer.team, gamePlayer.isLocalPlayer);
        else
        {
            gamePlayer.OnSetPlayerInfoTransfer += () => arm.SetupArm(gamePlayer.team, gamePlayer.isLocalPlayer);
        }

        if (!gamePlayer.isLocalPlayer)
            return;

        CanvasInstance.instance.weaponsToChose.GetComponentInChildren<ChoosingWeapon>().OnActivateWeapons += SetSelectedWeaponsIndexes;


        audioSync = GetComponent<AudioSync>();
        weaponAudioSource = weaponHolder.GetComponent<AudioSource>();

        controls = new PlayerControls();
        //WeaponInventory.instance.OnWeaponchange.Invoke(0); 
        controls.Player.Enable();
        controls.Player.WeaponInventory.performed += GetWeaponIndex;
    }

    private void OnDestroy()
    {
        if(gamePlayer.isLocalPlayer)
            controls?.Player.Disable();

        gamePlayer.OnSetPlayerInfoTransfer -= () => arm.SetupArm(gamePlayer.team, gamePlayer.isLocalPlayer);

        CanvasInstance.instance.weaponsToChose.GetComponentInChildren<ChoosingWeapon>().OnActivateWeapons += SetSelectedWeaponsIndexes;
    }

    private void Update()
    {
        if (!isLocalPlayer || !currentWeapon || !weaponsWasSelected || !canShoot)
            return;

        KeyCodes();
    }


    public void ChangeWeapon(int index, int currentIndex)
    {
        if (index == 0)
        {
            index = activatedWeaponsIndexes.Min();
        }
        else
        {
            index = activatedWeaponsIndexes.Max();
        }

        if (index < 0 || index >= weapons.Length || index == currentWeaponIndex)
            return;

        if (currentWeaponIndex == -1)
        {
            currentWeaponIndex = activatedWeaponsIndexes.Min();
        }

        if (currentWeapon)
        {
            currentWeapon.wasChanged = true;

        }

        weapons[currentWeaponIndex].SetActive(false);
        CmdChangeWeapon(currentWeaponIndex, index);
        WeaponInventory.instance.ChangeBlurIcon(index, currentWeaponIndex);
        currentWeaponIndex = index;
        
        weapons[currentWeaponIndex].SetActive(true);

        currentWeapon = weapons[index].GetComponent<Weapon>();
        currentWeapon.wasChanged = false;
        currentWeapon._isLocalPLayer = true;
        ChangeAnotherValuesAfterChangeWeapon();

        currentWeapon.weaponAmmo.UpdateAmmoOnScreen();
    }



    [Command(requiresAuthority = false)]
    public void CmdChangeWeapon(int currentIndex, int newIndex)
    {
        for (int i = 0; i < weaponHolder.childCount; i++)
        {
            if(weaponHolder.GetChild(i).gameObject == weapons[newIndex].gameObject)
            {
                currentWeaponChildIndex = i; 
                break;
            }

        }
        RpcChangeWeapon(currentIndex, newIndex);
    }

    [ClientRpc]
    private void RpcChangeWeapon(int currentIndex, int newIndex)
    {
        if (gamePlayer.isLocalPlayer)
            return;
        weapons[currentIndex].SetActive(false);
        weapons[newIndex].SetActive(true);

        currentWeapon = weapons[newIndex].GetComponent<Weapon>();

        ChangeAnotherValuesAfterChangeWeapon();
    }

    public void GetWeaponIndex(InputAction.CallbackContext context)
    {
        int index = (int)context.ReadValue<float>();

        if (!weaponsWasSelected || !canShoot)
            return;

        ChangeWeapon(index, index == 0 ? 1 : 0);
    }

    public void ChangeAnotherValuesAfterChangeWeapon()
    {
        multiNetworkComponentsSorter.GetNetworkAnimator("WeaponNetworkAnimator").animator = currentWeapon.GetComponentInChildren<Animator>();
        multiNetworkComponentsSorter.GetNetworkAnimator("WeaponNetworkAnimator").SetValues();
        //print($"muzzle position was been here    =           {currentWeapon.muzzlePosition!=null}");
        GetComponent<PlayerVFX>().muzzlePosition = currentWeapon.muzzlePosition;

        if (!isLocalPlayer)
            return;

        weaponAudioSource.Stop();

        WeaponSway _sway = weaponHolder.GetComponent<WeaponSway>();
        _sway.weapon = currentWeapon.transform;
        _sway.normalWeaponPosition = currentWeapon.initialWeaponPosition;

        //audioSource.PlayOneShot(changeWeaponClip); 
    }

    public void SetSelectedWeaponsIndexes(int first, int second)
    {
        activatedWeaponsIndexes.Add(first);
        activatedWeaponsIndexes.Add(second);
    }

    public void ToggleCanShoot(bool toggle)
    {
        canShoot = toggle;
    }

    public void KeyCodes()
    {
        if (controls.Player.ArmPunch.WasPerformedThisFrame())
        {
            if (arm.reloadTimer > arm.reloadTime)
            {
                gamePlayer.Punch(arm.Punch(), arm.damageToPlayer, arm.damageToOre, arm.punchDistance, arm.punchRadius, arm.hitLM, gamePlayer.GetNetID());
            }

        }

        if (currentWeapon.canShoot && !currentWeapon.reloading)
        {
            if (currentWeapon.weaponAmmo.Ammo > 0)
            {
                if (currentWeapon.shootTimer > currentWeapon.weaponScriptableObject.timeBetweenShots)
                {
                    if (currentWeapon.shootType == ShootType.Auto && controls.Player.Fire.IsPressed())
                    {
                        //audioSync.PlaySound(0);
                        //print(gamePlayer.GetNetID());
                        StartCoroutine(gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.shootRange, gamePlayer.GetNetID(), currentWeapon.weaponScriptableObject.timeBetweenSpawnBullets));
                    }
                    else if (currentWeapon.shootType == ShootType.Semi && controls.Player.Fire.WasPerformedThisFrame())
                    {
                        //audioSync.PlaySound(0);
                        //print(gamePlayer.GetNetID());
                        StartCoroutine(gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.shootRange, gamePlayer.GetNetID(), currentWeapon.weaponScriptableObject.timeBetweenSpawnBullets));

                    }
                }
            }
        }

        if (controls.Player.Fire.WasPressedThisFrame() && currentWeapon.weaponAmmo.Ammo <= 0)
        {
            audioSync.PlaySound(ClipType.weapon,true, $"{currentWeapon.weaponName}_Empty");
        }

        if (controls.Player.Reload.WasPerformedThisFrame() && !currentWeapon.reloading)
        {
            if (currentWeapon.weaponAmmo.Ammo < currentWeapon.weaponScriptableObject.maxAmmo)
            {
                //audioSync.PlaySound(ClipType.weapon,true, $"{currentWeapon.weaponName}_Reload");
                StartCoroutine(currentWeapon.ReloadCoroutine());
                
            }
        }

    }



    [ClientRpc]
    private void PlaySoundOnClients(PlaySoundType type)
    {
        currentWeapon.PlaySound(type);
    }
    [Command]
    private void PlaySoundCommand(PlaySoundType type)
    {
        PlaySoundOnClients(type);
    }

}