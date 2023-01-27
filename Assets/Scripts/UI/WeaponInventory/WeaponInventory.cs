using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public static WeaponInventory instance;
    [SerializeField] private GameObject[] weaponBlurIcons;
    [SerializeField] private int activeWeaponIndex;
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Debug.LogError("MORE THAN ONE INSTANCE OF WEAPON INVENTORY");
        }

    }


    public void ChangeBlurIcon(int indexToChange, int currentIndex)
    {
        print($"bluuur     index - {indexToChange}");
        print($"bluuur     current index - {currentIndex}");
        weaponBlurIcons[currentIndex].SetActive(true);
        weaponBlurIcons[indexToChange].SetActive(false);
    }
    
}
