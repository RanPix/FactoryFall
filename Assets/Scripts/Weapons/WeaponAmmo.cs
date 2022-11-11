using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class WeaponAmmo : MonoBehaviour
{
    public TMP_Text AmmoText;
    public Canvas canvas;
    public int ClipSize;
    public int Ammo;
    public int ReserveAmmo;
    private GameObject help;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<Canvas>();
        help = Instantiate(AmmoText.gameObject, canvas.transform);
    }
    private void Start()
    {
        AmmoText = help.GetComponent<TMP_Text>();
    }
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
        ApdateAmmoInScreen();
    }
    public void ApdateAmmoInScreen()
    {
        AmmoText.text = Ammo + "/" + ReserveAmmo;
        if (Ammo <= 0) Ammo = 0;
        if (ReserveAmmo <= 0) ReserveAmmo = 0;
    }
}

