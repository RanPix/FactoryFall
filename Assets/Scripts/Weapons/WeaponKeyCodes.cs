using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKeyCodes : MonoBehaviour
{
    public Weapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyCodes();
    }
    public void KeyCodes()
    {
        if (weapon.canShoot == true)
        {
            if (weapon.weaponAmmo.Ammo > 0)
            {

                if (Time.time - weapon.nextFire > 1 / weapon.weaponScriptableObject.fireRate)
                {
                    if (weapon._shootType == ShootType.Auto && weapon.controls.Player.Fire.IsPressed())
                    {
                        weapon.Shoot();
                    }
                    else if (weapon._shootType == ShootType.Semi && weapon.controls.Player.Fire.WasPerformedThisFrame())
                    {
                        weapon.Shoot();

                    }
                    else if (weapon._shootType == ShootType.Auto && weapon.controls.Player.Fire.WasReleasedThisFrame())
                    {
                        weapon.FireButtonWasReleased();
                    }
                }
            }
        }
        if (weapon.controls.Player.Fire.IsPressed() && weapon.weaponAmmo.Ammo <= 0)
        {
            weapon.audioSource.PlayOneShot(weapon.weaponScriptableObject.empty);
        }
        if (weapon.controls.Player.Reload.WasPerformedThisFrame())
        {
            if (weapon.weaponAmmo.ReserveAmmo > 0 && weapon.weaponAmmo.Ammo < weapon.ammo)
            {
                StartCoroutine(weapon.ReloadCoroutine());
            }
        }

    }

}
