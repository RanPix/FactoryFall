using UnityEngine;
using TMPro;

public class WeaponAmmo : MonoBehaviour
{
    public TMP_Text AmmoText;
    public int ClipSize;

    public int Ammo = 0;
    public int ReserveAmmo;

    public void ResetAmmo()
    {
        Ammo = ClipSize;
        UpdateAmmoOnScreen();
    }
    public void SetAmmo(int count)
    {
        Ammo = count;
        UpdateAmmoOnScreen();
    }
    public void AddAmmo(int count)
    {
        Ammo += count;
        UpdateAmmoOnScreen();
    }
    public void UpdateAmmoOnScreen()
    {
        AmmoText.text = Ammo.ToString();
        if (Ammo <= 0) Ammo = 0;
        if (ReserveAmmo <= 0) ReserveAmmo = 0;
    }
}

