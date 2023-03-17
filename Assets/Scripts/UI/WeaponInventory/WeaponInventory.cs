using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private RawImage[] weaponBlurIcons;
    [SerializeField] private RawImage[] unSelectedWeaponBlurIcons;
    [SerializeField] private int activeWeaponIndex;


    public void ChangeIcon(int indexToChange, int currentIndex)
    {
        weaponBlurIcons[currentIndex].enabled = true;
        weaponBlurIcons[indexToChange].enabled = false;

        unSelectedWeaponBlurIcons[currentIndex].enabled = false;
        unSelectedWeaponBlurIcons[indexToChange].enabled = true;
    }
    
}
