using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChosingWeapon : MonoBehaviour
{
    public List<GameObject> weaponsInventoryItems = new List<GameObject>();
    private int selectedWeaponsCount;
    private int maxWeaponsCount;
    public bool canSelectAnotherWeapon = true;
    public Action<int, int> OnActivateWeapons;
    void Start()
    {
        CursorManager.SetCursorLockState(CursorLockMode.None);
        CursorManager.disablesToLockCount++;
        Menu.Instance.canOpenMenu = false;
        Menu.Instance.look.canRotateCamera = false;
        CanvasInstance.instance.tabBar.canOpen = false;

    }

    // Update is called once per frame

    public void OnWeaponClick(bool wasSelected)
    {
        if (wasSelected)
        {
            if (canSelectAnotherWeapon)
            {
                selectedWeaponsCount++;
            }
        }
        else
        {
            selectedWeaponsCount--;
        }

        if (selectedWeaponsCount >= 2)
        {
            ActivateSelectedWeapons();
            gameObject.SetActive(false);
        }
    }

    private void ActivateSelectedWeapons()
    {
        int firstIndex = 0;
        int secondIndex = 0;
        for (int i = 0; i < 2; i++)
        {
            weaponsInventoryItems[i].SetActive(true);
        }

        
        for (int i = 0; i < CanvasInstance.instance.weaponInventory.transform.childCount; i++)
        {
            if (weaponsInventoryItems[0] == CanvasInstance.instance.weaponInventory.transform.GetChild(i).gameObject)
            {
                firstIndex = i;
            }else if (weaponsInventoryItems[1] == CanvasInstance.instance.weaponInventory.transform.GetChild(i).gameObject)
            {
                secondIndex = i;
            }
        }
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<WeaponKeyCodes>().weaponsWasSelected = true;
        CursorManager.disablesToLockCount--;
        Menu.Instance.look.canRotateCamera = true;
        Menu.Instance.canOpenMenu = true;
        CanvasInstance.instance.tabBar.canOpen = true;

        CursorManager.SetCursorLockState(CursorLockMode.Locked);
        
        OnActivateWeapons?.Invoke(firstIndex, secondIndex);

    }
    
}
