using UnityEngine;
using TMPro;

public class ConnectorHelper : MonoBehaviour
{
    public static ConnectorHelper Instance { get; private set; }
    public GameObject weaponHolder;
    public Camera gunCam;
    public GameObject player;
    public TMP_Text ammoText;
    public WeaponAmmo weaponAmmo;
    void Awake()
    {
        Instance = this;
    }

}
