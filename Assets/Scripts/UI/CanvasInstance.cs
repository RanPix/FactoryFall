using UnityEngine;

public class CanvasInstance : MonoBehaviour
{
    
    public static CanvasInstance instance;

    public GameObject canvas;
    public RectTransform panelWithElementsToHide;
    public GameObject hitMarker;
    public WeaponInventory weaponInventory;
    public KillFeed killFeed;
    public GameObject weaponAmmoText;
    public ChosingWeapon weaponsToChose;
    public GameObject scoreBoard;
    public OreInventory oreInventory;
    public TabBar tabBar;
    public WeaponInfo selectedWeaponInfo;
    public MainChat mainChat;
    public GameObject damageVingette;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }
}
