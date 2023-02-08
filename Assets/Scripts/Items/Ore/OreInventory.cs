using UnityEngine;

public class OreInventory : MonoBehaviour
{
    public OreInventoryItem item;

    private void Awake()
    {
        if (GameManager.instance.matchSettings.gm != Gamemode.BTR)
            gameObject.SetActive(false);

    }
}
