using UnityEngine;

public class OreInventory : MonoBehaviour
{
    public OreInventoryItem item;

    private void Awake()
    {
        GameManager.instance.OnMatchSettingsSettedup += Setup;

    }

    private void OnDestroy()
    {
        GameManager.instance.OnMatchSettingsSettedup -= Setup;
    }

    private void Setup(bool teamsMatch)
    {
        print(GameManager.instance.matchSettings.gm);

        if (GameManager.instance.matchSettings.gm != Gamemode.BTR)
        {
            print("HOW");
            gameObject.SetActive(false);
        }
    }
}
