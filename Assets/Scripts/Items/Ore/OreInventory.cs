using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Player;

public class OreInventory : MonoBehaviour
{
    public OreInventoryItem item;
    [SerializeField] private RawImage oreImage;

    private void Awake()
    {
        if (GameManager.instance.matchSettings.gm != Gamemode.BTR)
            gameObject.SetActive(false);
    }

    private void Start()
        => oreImage.color = NetworkClient.localPlayer.GetComponent<GamePlayer>().team == Team.Red ? Color.red : Color.blue;
}
