using System;
using UnityEngine;
using UnityEngine.UI;

public class ChosingWeaponItem : MonoBehaviour
{
    [SerializeField] private GameObject weaponInventoryItem;
    [SerializeField] private RawImage image;

    [SerializeField] private WeaponScriptableObject weaponScriptableObject;


    public bool wasSelected = false;

    public void OnCursorEnter()
    {
        image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        transform.GetComponentInParent<ChoosingWeapon>().OnActivate?.Invoke(weaponScriptableObject.type, weaponScriptableObject.name, weaponScriptableObject.damage, weaponScriptableObject.timeBetweenShots, weaponScriptableObject.shootRange, weaponScriptableObject.numberOfBulletsPerShot);
    }

    public void OnCursorExit()
    {
        if(wasSelected)
            return;
        image.color = new Color(1f, 1f, 1f, 1f);
    }

    public void OnCursorClick()
    {
        print("i klic");
        if (CanvasInstance.instance.weaponsToChose.canSelectAnotherWeapon || wasSelected)
        {
            print("i fi 1");
            wasSelected = !wasSelected;
            if (wasSelected)
            {
                print("i selected");

                CanvasInstance.instance.weaponsToChose.weaponsInventoryItems.Add(weaponInventoryItem);
                transform.GetComponentInParent<ChoosingWeapon>().OnActivate?.Invoke(weaponScriptableObject.type, weaponScriptableObject.name, weaponScriptableObject.damage, weaponScriptableObject.timeBetweenShots, weaponScriptableObject.shootRange, weaponScriptableObject.numberOfBulletsPerShot);

            }
            else
            {
                print("i de selected");
                CanvasInstance.instance.weaponsToChose.weaponsInventoryItems.Remove(weaponInventoryItem);
            }

        }
        CanvasInstance.instance.weaponsToChose.OnWeaponClick(wasSelected);
    }
}
