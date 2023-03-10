using System;
using UnityEngine;
using TMPro;

public class WeaponAmmo : MonoBehaviour
{
    public TMP_Text AmmoText;
    public int ClipSize;

    public int Ammo
    {
        get => _ammo;
        set
        {
            _ammo = value;
            OnAmmoChange?.Invoke(value);
        }
    }
    private int _ammo = 0;

    public Action<int> OnAmmoChange;

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
        if (Ammo <= 0) Ammo = 0;
        AmmoText.text = Ammo.ToString();
    }
}

