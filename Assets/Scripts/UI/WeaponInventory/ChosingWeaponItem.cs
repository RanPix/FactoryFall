using System;
using UnityEngine;

public class ChosingWeaponItem : MonoBehaviour
{
    [SerializeField] private GameObject weaponInventoryItem;
    [SerializeField] private GameObject blur;
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
        if (CanvasInstance.instance.weaponsToChose.canSelectAnotherWeapon || wasSelected)
        {
            wasSelected = wasSelected ? false:true;
            if(wasSelected)
                CanvasInstance.instance.weaponsToChose.weaponsInventoryItems.Add(weaponInventoryItem);
            else
                CanvasInstance.instance.weaponsToChose.weaponsInventoryItems.Remove(weaponInventoryItem);

        }
        CanvasInstance.instance.weaponsToChose.OnWeaponClick(wasSelected);
    }
}
