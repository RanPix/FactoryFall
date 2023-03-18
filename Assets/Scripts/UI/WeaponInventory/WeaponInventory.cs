using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    public Transform otherFrame;
    public Transform currentFrame;

    [SerializeField] private RawImage[] weaponIcons;
    [SerializeField] private RawImage[] unSelectedWeaponIcons;
    [SerializeField] private int activeWeaponIndex;

    private void Start()
    {
        for (int i = 0; i < otherFrame.childCount; i++)
        {
            if (otherFrame.GetChild(i).name != currentFrame.GetChild(i).name)
            {
                Debug.LogError($"Arr of otherFrame children`s names aren`t equal to arr of currentFrame children`s names");
            }
        }
    }

    public void ChangeIcon(int indexToChange, int currentIndex)
    {
        weaponIcons[currentIndex].enabled = true;
        weaponIcons[indexToChange].enabled = false;

        unSelectedWeaponIcons[currentIndex].enabled = false;
        unSelectedWeaponIcons[indexToChange].enabled = true;
    }
    
}
