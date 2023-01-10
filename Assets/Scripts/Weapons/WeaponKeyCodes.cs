using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;
[RequireComponent(typeof(AudioSync))]
public class WeaponKeyCodes : NetworkBehaviour
{
    public Transform weaponHolder;
    public Weapon currentWeapon;
    private int currentWeaponIndex = -1;
    private GamePlayer gamePlayer;
    public PlayerControls controls { get; private set; }

    private AudioSync audioSync;
    // Start is called before the first frame update
    void Start()
    {
        if(!isLocalPlayer)
            return;
        audioSync = GetComponent<AudioSync>();
        gamePlayer = GetComponent<GamePlayer>();
        controls = new PlayerControls();
        controls.Player.Enable();
        ChangeWeapon(0);
    }

    // Update is called once per frame
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
        currentWeapon._isLocalPLayer = true;
        currentWeaponIndex = index;
        currentWeapon._isLocalPLayer = true;
    }
    public void KeyCodes()
    {
        if (currentWeapon.canShoot == true)
        {
            if (currentWeapon.weaponAmmo.Ammo > 0)
            {
                if (Time.time - currentWeapon.nextFire > 1 / currentWeapon.weaponScriptableObject.fireRate)
                {
                    if (currentWeapon._shootType == ShootType.Auto && controls.Player.Fire.IsPressed())
                    {
                        audioSync.PlaySound(0);
                        gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.playerMask, currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.weaponShootRange, gamePlayer.GetLocalNetID());
                    }
                    else if (currentWeapon._shootType == ShootType.Semi && controls.Player.Fire.WasPerformedThisFrame())
                    {
                        audioSync.PlaySound(0);
                        gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.playerMask, currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.weaponShootRange, gamePlayer.GetLocalNetID());

                    }
                    else if (currentWeapon._shootType == ShootType.Auto && controls.Player.Fire.WasReleasedThisFrame())
                    {
                        currentWeapon.FireButtonWasReleased();
                        gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.playerMask, currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.weaponShootRange, gamePlayer.GetLocalNetID());

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
