using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;

public class WeaponKeyCodes : NetworkBehaviour
{
    public Transform weaponHolder;
    public Weapon currentWeapon;
    private int currentWeaponIndex = -1;
    private GamePlayer gamePlayer;
    public PlayerControls controls { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if(!isLocalPlayer)
            return;
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
                        gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.playerMask, currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.weaponShootRange);
                    }
                    else if (currentWeapon._shootType == ShootType.Semi && controls.Player.Fire.WasPerformedThisFrame())
                    {
                        gamePlayer.Shoot(currentWeapon.Shoot(), currentWeapon.playerMask, currentWeapon.weaponScriptableObject.damage, currentWeapon.weaponScriptableObject.weaponShootRange);

                    }
                    else if (currentWeapon._shootType == ShootType.Auto && controls.Player.Fire.WasReleasedThisFrame())
                    {
                        currentWeapon.FireButtonWasReleased();
                    }else if (currentWeapon._shootType == ShootType.Burst && controls.Player.Fire.IsPressed())
                    {

                    }
                }
            }
        }

        if (controls.Player.Fire.WasPressedThisFrame() && currentWeapon.weaponAmmo.Ammo <= 0)
        {
            currentWeapon.audioSource.PlayOneShot(currentWeapon.weaponScriptableObject.empty);
        }

        if (controls.Player.Reload.WasPerformedThisFrame() && !currentWeapon.reloading)
        {
            if (currentWeapon.weaponAmmo.ReserveAmmo > 0 && currentWeapon.weaponAmmo.Ammo < currentWeapon.ammo)
            {
                StartCoroutine(currentWeapon.ReloadCoroutine());
            }
        }

    }

}
