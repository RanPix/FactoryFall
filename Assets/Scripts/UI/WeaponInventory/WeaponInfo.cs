using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text fireRateText;
    [SerializeField] private TMP_Text shootRangeText;
    [SerializeField] private TMP_Text bulletsPerShotText;

    [SerializeField] private List<GameObject> weaponsIcons = new List<GameObject>();
    [SerializeField] private List<string> weaponsNames = new List<string>();
    [SerializeField] private Dictionary<string, GameObject> weaponsIconsDctionary = new Dictionary<string, GameObject>();

    [SerializeField] private RawImage iconRawImage;
    [SerializeField] private GameObject infoPanel;

    private string lastName = "";
    void Start()
    {
        if(weaponsIcons.Count != weaponsNames.Count)
            Debug.LogError("The number of weapon icons must equal the number of weapon names");
        for (int i = 0; i < weaponsIcons.Count; i++)
        {
            weaponsIconsDctionary.Add(weaponsNames[i], weaponsIcons[i]);
        }

        CanvasInstance.instance.weaponsToChose.OnActivate += UpdateParams;
    }

    public void UpdateParams(string type, string name, float damage, float fireRate, float shootRange, int bulletsPerShot)
    {
        if(!weaponsNames.Contains(name))
            Debug.LogError("There is no weapon with this name");
        if (weaponsIconsDctionary.Keys.Contains(lastName))
        {
            weaponsIconsDctionary[lastName].SetActive(false);

        }
        weaponsIconsDctionary[name].SetActive(true);
        infoPanel.SetActive(true);
        GetComponent<Image>().enabled = true;
        lastName = name;

        typeText.text = $"Type: {type}";
        nameText.text = $"Name: {name}";
        damageText.text = $"Damage: {damage}";
        fireRateText.text = $"Fire rate: {fireRate}";
        shootRangeText.text = $"Shoot range: {shootRange}";
        bulletsPerShotText.text = $"Bullets per shot: {bulletsPerShot}";
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
