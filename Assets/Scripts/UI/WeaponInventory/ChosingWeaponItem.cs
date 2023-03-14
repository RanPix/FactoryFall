using System;
using UnityEngine;

public class ChosingWeaponItem : MonoBehaviour
{
    [SerializeField] private GameObject weaponInventoryItem;
    [SerializeField] private GameObject blur;

    [SerializeField] private WeaponScriptableObject weaponScriptableObject;


    public bool wasSelected = false;

    public void OnCursorEnter()
    {
        blur.SetActive(false);
        transform.GetComponentInParent<ChoosingWeapon>().OnActivate?.Invoke(weaponScriptableObject.type, weaponScriptableObject.name, weaponScriptableObject.damage, weaponScriptableObject.timeBetweenShots, weaponScriptableObject.shootRange, weaponScriptableObject.numberOfBulletsPerShot);
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
            wasSelected = !wasSelected;
            if (wasSelected)
            {
                CanvasInstance.instance.weaponsToChose.weaponsInventoryItems.Add(weaponInventoryItem);
                transform.GetComponentInParent<ChoosingWeapon>().OnActivate?.Invoke(weaponScriptableObject.type, weaponScriptableObject.name, weaponScriptableObject.damage, weaponScriptableObject.timeBetweenShots, weaponScriptableObject.shootRange, weaponScriptableObject.numberOfBulletsPerShot);

            }
            else
            {
                CanvasInstance.instance.weaponsToChose.weaponsInventoryItems.Remove(weaponInventoryItem);
            }

        }
        CanvasInstance.instance.weaponsToChose.OnWeaponClick(wasSelected);
    }
}
