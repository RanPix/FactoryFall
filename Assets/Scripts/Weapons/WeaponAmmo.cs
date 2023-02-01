using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class WeaponAmmo : MonoBehaviour
{
    public TMP_Text AmmoText;
    public int ClipSize;
    public int Ammo = 0;
    public int ReserveAmmo;

    private void Awake()
    {
    }
    private void Start()
    {
    }
    public void AddAmmo()
    {
        int amountNeeded = ClipSize - Ammo;
        if (amountNeeded >= ReserveAmmo)
        {
            Ammo += ReserveAmmo;
            //ReserveAmmo -= amountNeeded;
        }
        else
        {
            Ammo = ClipSize;
            //ReserveAmmo -= amountNeeded;
        }
        UpdateAmmoInScreen();
    }
    public void UpdateAmmoInScreen()
    {
        AmmoText.text = Ammo.ToString();
        if (Ammo <= 0) Ammo = 0;
        if (ReserveAmmo <= 0) ReserveAmmo = 0;
    }
}

