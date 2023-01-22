using System.Collections;
using Mirror;
using Player;
using UnityEngine;
using Weapons;

[RequireComponent(typeof(AudioSync))]
public class WeaponKeyCodes : NetworkBehaviour
{
    [SerializeField] private GamePlayer gamePlayer;
    public Transform weaponHolder;
    public Weapon currentWeapon;
    [SerializeField] private Arm arm;

    private int currentWeaponIndex = -1;
    public PlayerControls controls { get; private set; }

    private AudioSync audioSync;

    void Start()
    {
        if(!isLocalPlayer)
            return;

        arm._isLocalPLayer = true;

        audioSync = GetComponent<AudioSync>();
        controls = new PlayerControls();
        controls.Player.Enable();
        ChangeWeapon(0);
    }

    void Update()
    {
        if(!isLocalPlayer)
            return;

        KeyCodes();
    }



    public void ChangeWeapon(int index)
    {
        if(currentWeaponIndex == index)
            return;

        if(currentWeaponIndex == -1)
            currentWeaponIndex = 0;

        weaponHolder.GetChild(currentWeaponIndex).gameObject.SetActive(false);
        weaponHolder.GetChild(index).gameObject.SetActive(true);
       
        currentWeapon = weaponHolder.GetChild(index).GetComponent<Weapon>();
        currentWeaponIndex = index;
        currentWeapon._isLocalPLayer = true;
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
