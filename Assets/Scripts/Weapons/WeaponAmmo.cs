using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAmmo : MonoBehaviour
{
    public Text AmmoText;
    public int ClipSize;
    public int Ammo;
    public int ReserveAmmo;

    public void AddAmmo()
    {
        int amountNeeded = ClipSize - Ammo;
        if (amountNeeded >= ReserveAmmo)
        {
            Ammo += ReserveAmmo;
            ReserveAmmo -= amountNeeded;
        }
        else
        {
            Ammo = ClipSize;
            ReserveAmmo -= amountNeeded;
        }
        ApdataAmmoInScreen();
    }
    public void ApdataAmmoInScreen()
    {
        AmmoText.text = Ammo + "/" + ReserveAmmo;
        if (Ammo <= 0) Ammo = 0;
        if (ReserveAmmo <= 0) ReserveAmmo = 0;
    }
}

