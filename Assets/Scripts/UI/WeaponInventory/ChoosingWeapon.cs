using System;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class ChoosingWeapon : MonoBehaviour
{
    [HideInInspector] public List<GameObject> weaponsInventoryItems = new List<GameObject>();
    private int selectedWeaponsCount;
    private int maxWeaponsCount;
    public bool canSelectAnotherWeapon = true;
    public Action<int, int> OnActivateWeapons;

    public Action<string, string, float, float, float, int> OnActivate;

    public void Setup()
    {
        CursorManager.instance.SetCursorLockState(CursorLockMode.None);
        CursorManager.instance.disablesToLockCount++;

        Menu.Instance.canOpenMenu = false;

        Look _look = NetworkClient.localPlayer.GetComponent<GamePlayer>().cameraHolder.GetComponent<Look>();
        _look.canRotateCamera = false;

        CanvasInstance.instance.tabBar.canOpen = false;

        CanvasInstance.instance.tipsManager.ActivateTip("ChosingWeapon");
    }

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
        for (int i = 0; i < CanvasInstance.instance.weaponInventory.otherFrame.childCount; i++)
        {
            if (weaponsInventoryItems[0].name == CanvasInstance.instance.weaponInventory.otherFrame.GetChild(i).name)
            {
                CanvasInstance.instance.weaponInventory.otherFrame.GetChild(i).gameObject.SetActive(true);
                CanvasInstance.instance.weaponInventory.currentFrame.GetChild(i).gameObject.SetActive(true);
                CanvasInstance.instance.weaponInventory.currentFrame.GetChild(i).GetComponent<RawImage>().enabled = false;
            }
            else if (weaponsInventoryItems[1].name == CanvasInstance.instance.weaponInventory.otherFrame.GetChild(i).name)
            {
                CanvasInstance.instance.weaponInventory.currentFrame.GetChild(i).gameObject.SetActive(true);
                CanvasInstance.instance.weaponInventory.otherFrame.GetChild(i).gameObject.SetActive(true);
                CanvasInstance.instance.weaponInventory.otherFrame.GetChild(i).GetComponent<RawImage>().enabled = false;
            }
        }


        for (int i = 0; i < CanvasInstance.instance.weaponInventory.otherFrame.childCount; i++)
        {
            if (weaponsInventoryItems[0].name == CanvasInstance.instance.weaponInventory.otherFrame.GetChild(i).name)
            {
                firstIndex = i;
            }
            else if (weaponsInventoryItems[1].name == CanvasInstance.instance.weaponInventory.otherFrame.GetChild(i).name)
            {
                secondIndex = i;
            }
        }

        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<WeaponKeyCodes>().weaponsWasSelected = true;
        CursorManager.instance.disablesToLockCount--;
        Menu.Instance.look.canRotateCamera = true;
        Menu.Instance.canOpenMenu = true;
        CanvasInstance.instance.tabBar.canOpen = true;
        CanvasInstance.instance.selectedWeaponInfo.TurnOff();

        CursorManager.instance.SetCursorLockState(CursorLockMode.Locked);
        CanvasInstance.instance.tipsManager.ActivateTip("PressTab");

        OnActivateWeapons?.Invoke(firstIndex, secondIndex);

    }
    
}
