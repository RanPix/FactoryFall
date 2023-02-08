using UnityEngine;
using TMPro;

public class WeaponAmmo : MonoBehaviour
{
    public TMP_Text AmmoText;
    public int ClipSize;

    public int Ammo = 0;
    public int ReserveAmmo;

    public void AddAmmo()
    {
        Ammo = ClipSize;
        UpdateAmmoInScreen();
    }
    public void UpdateAmmoInScreen()
    {
        AmmoText.text = Ammo.ToString();
        if (Ammo <= 0) Ammo = 0;
        if (ReserveAmmo <= 0) ReserveAmmo = 0;
    }
}

