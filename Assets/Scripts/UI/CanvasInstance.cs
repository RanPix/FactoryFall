using UnityEngine;

public class CanvasInstance : MonoBehaviour
{
    
    public static CanvasInstance instance;

    public GameObject canvas;
    public GameObject hitMarker;
    public GameObject weaponInventory;
    public GameObject killFeed;
    public GameObject weaponAmmoText;
    public GameObject weaponsToChose;
    public GameObject scoreBoard;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }
}
