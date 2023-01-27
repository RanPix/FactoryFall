using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class WeaponInventoryItem : MonoBehaviour
{
    [SerializeField] private string weaponName;
    [SerializeField] private Weapon ownWeapon;
    public int GetWeaponGO()
    {
        GameObject localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");
        WeaponKeyCodes weaponKeyCodes = localPlayer.GetComponent<WeaponKeyCodes>();
        int ownIndex = 0;
        if (!ownWeapon)
        {
            for (int i = 0; i < weaponKeyCodes.weaponHolder.childCount; i++)
            {
                if (weaponKeyCodes.weaponHolder.GetChild(i).name == weaponName)
                {
                    ownWeapon = weaponKeyCodes.weaponHolder.GetChild(i).GetComponent<Weapon>();
                }
            }
        }
        return ownWeapon.weaponIndex;
        
    }
}
