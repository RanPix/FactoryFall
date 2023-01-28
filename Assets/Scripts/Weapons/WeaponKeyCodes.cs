using System.Collections.Generic;
using System.Linq;
using Mirror;
using Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

[RequireComponent(typeof(AudioSync))]
public class WeaponKeyCodes : NetworkBehaviour
{
    [SerializeField] private GamePlayer gamePlayer;
    [SerializeField] private Arm arm;

    public Transform weaponHolder;
    public Weapon currentWeapon;
    public int currentWeaponIndex = -1;
    public GameObject[] weapons;
    public List<int> activatedWeaponsIndexes = new List<int>();
    public PlayerControls controls { get; private set; }

    private AudioSync audioSync;

    void Start()
    {
        if (!isLocalPlayer)
            return;
        CanvasInstance.instance.weaponsToChose.GetComponentInChildren<ChosingWeapon>().OnAñtivateWeapons += SetSelectedWeaponsIndexes;
        arm._isLocalPLayer = true;
        audioSync = GetComponent<AudioSync>();
        controls = new PlayerControls(); 
        //WeaponInventory.instance.OnWeaponchange.Invoke(0);
        controls.Player.Enable();
        controls.Player.WeaponInventory.performed += GetWeaponIndex;

    }

    void Update()
    {
        if(!isLocalPlayer || !currentWeapon)
            return;

        KeyCodes();
    }


    public void ChangeWeapon(int index, int currentIndex)
    {
        if(index < 0 || index >= weapons.Length)
            return;
        if (currentWeaponIndex == -1)
        {
            currentWeaponIndex = activatedWeaponsIndexes.Min();
        }

        if (index==0)
        {
            index = activatedWeaponsIndexes.Min();
        }
        else
        {
            index = activatedWeaponsIndexes.Max();
        }

        weapons[currentWeaponIndex].SetActive(false);
        weapons[index].SetActive(true);
       
        WeaponInventory.instance.ChangeBlurIcon(index, currentIndex);
        currentWeapon = weapons[index].GetComponent<Weapon>();
        currentWeaponIndex = index;
        currentWeapon._isLocalPLayer = true;
        //currentWeapon.UpdateAmmo();
        ChangeAnotherValuesAfterChangeWeapon();
    }

    public void GetWeaponIndex(InputAction.CallbackContext context)
    {
        int index = (int)context.ReadValue<float>();
        ChangeWeapon(index, index==0?1:0);
    }

    public void ChangeAnotherValuesAfterChangeWeapon()
    {
        GetComponent<NetworkTransformChild>().target = currentWeapon.transform;
        GetComponent<NetworkAnimator>().animator = currentWeapon.GetComponentInChildren<Animator>();
        GetComponent<NetworkAnimator>().SetValues();
        GetComponent<GamePlayer>().muzzlePosition = currentWeapon.muzzlePosition;
    }

    public void SetSelectedWeaponsIndexes(int first, int second)
    {
        activatedWeaponsIndexes.Add(first);
        activatedWeaponsIndexes.Add(second);
    }



    public void KeyCodes()
    {
        if (controls.Player.ArmPunch.WasPerformedThisFrame())
        {
            if (arm.reloadTimer > arm.reloadTime)
            {
                gamePlayer.Punch(arm.Punch(), arm.damage, arm.punchDistance, arm.punchRadius, arm.hitLM, gamePlayer.GetLocalNetID());
            }

        }

        if (currentWeapon.canShoot == true)
        {
            if (currentWeapon.weaponAmmo.Ammo > 0)
            {
                if (Time.time - currentWeapon.nextFire > 1 / currentWeapon.weaponScriptableObject.fireRate)
                {
                    if (currentWeapon._shootType == ShootType.Auto && controls.Player.Fire.IsPressed())
                    {
                        audioSync.PlaySound(0);
                        gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.weaponShootRange, gamePlayer.GetLocalNetID());
                    }
                    else if (currentWeapon._shootType == ShootType.Semi && controls.Player.Fire.WasPerformedThisFrame())
                    {
                        audioSync.PlaySound(0);
                        gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.weaponShootRange, gamePlayer.GetLocalNetID());

                    }
                    else if (currentWeapon._shootType == ShootType.Auto && controls.Player.Fire.WasReleasedThisFrame())
                    {
                        currentWeapon.FireButtonWasReleased();
                        currentWeapon.Shoot();
                        gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.weaponShootRange, gamePlayer.GetLocalNetID());

                    }
                }
            }
        }

        if (controls.Player.Fire.WasPressedThisFrame() && currentWeapon.weaponAmmo.Ammo <= 0)
        {
            audioSync.PlaySound(1);
        }

        if (controls.Player.Reload.WasPerformedThisFrame() && !currentWeapon.reloading)
        {
            if (currentWeapon.weaponAmmo.ReserveAmmo > 0 && currentWeapon.weaponAmmo.Ammo < currentWeapon.ammo)
            {
                audioSync.PlaySound(2);
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
