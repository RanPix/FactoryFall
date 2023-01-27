using System;
using UnityEngine;

public class ChosingWeaponItem : MonoBehaviour
{
    [SerializeField] private GameObject weaponInventoryItem;
    [SerializeField] private GameObject blur;
    [SerializeField] private ChosingWeapon chosingWeapon;
    public bool wasSelected = false;

    public void OnCursorEnter()
    {
        blur.SetActive(false);
    }

    public void OnCursorExit()
    {
        if(wasSelected)
            return;
        blur.SetActive(true);
    }

    public void OnCursorClick()
    {
        if (chosingWeapon.canSelectAnotherWeapon || wasSelected)
        {
            wasSelected = wasSelected ? false:true;
            if(wasSelected)
                chosingWeapon.weaponsInventoryItems.Add(weaponInventoryItem);
            else
                chosingWeapon.weaponsInventoryItems.Remove(weaponInventoryItem);

        }
        chosingWeapon.OnWeaponClick(wasSelected);
    }
}
