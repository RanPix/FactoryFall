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
        int secondIndex = 1;
        for (int i = 0; i < 2; i++)
        {
            weaponsInventoryItems[i].gameObject.SetActive(true);
        }

        Transform parent = weaponsInventoryItems[0].transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (weaponsInventoryItems[0] == parent.GetChild(i))
            {
                firstIndex = i;
            }else if (weaponsInventoryItems[1] == parent.GetChild(i))
            {
                secondIndex = i;
            }
        }
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<WeaponKeyCodes>().weaponsWasSelected = true;
        CursorManager.disablesToLockCount--;
        Menu.Instance.look.canRotateCamera = true;
        Menu.Instance.canOpenMenu = true;
        CursorManager.SetCursorLockState(CursorLockMode.Locked);
        OnActivateWeapons?.Invoke(firstIndex, secondIndex);

    }
    
}
