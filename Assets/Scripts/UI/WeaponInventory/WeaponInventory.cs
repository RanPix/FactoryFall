using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public static WeaponInventory instance;
    [SerializeField] public GameObject[] weapons = new GameObject[2];
    [SerializeField] private GameObject[] weaponBlurIcons;
    [SerializeField] private int activeWeaponIndex;
    public Action<int> OnWeaponchange;
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Debug.LogError("MORE THAN ONE INSTANCE OF WEAPON INVENTORY");
        }

        OnWeaponchange += ChangeBlurIcon;
    }


    public void ChangeBlurIcon(int indexToChange)
    {
        weaponBlurIcons[activeWeaponIndex].SetActive(true);
        weaponBlurIcons[indexToChange].SetActive(true);
    }
    
}
