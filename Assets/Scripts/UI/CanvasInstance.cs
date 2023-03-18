using UnityEngine;

public class CanvasInstance : MonoBehaviour
{
    
    public static CanvasInstance instance;

    public GameObject canvas;
    public GameObject hitMarker;
    public WeaponInventory weaponInventory;
    public KillFeed killFeed;
    public GameObject weaponAmmoText;
    public ChoosingWeapon weaponsToChose;
    public GameObject scoreBoard;
    public OreInventory oreInventory;
    public TabBar tabBar;
    public WeaponInfo selectedWeaponInfo;
    public MainChat mainChat;
    public GameObject damageVingette;
    public TipsManager tipsManager;
    public Menu menu;
    public InGameUIDisabler inGameUIDisabler;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }
}
