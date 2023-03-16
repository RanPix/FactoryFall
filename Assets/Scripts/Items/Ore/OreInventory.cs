using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Player;

public class OreInventory : MonoBehaviour
{
    public OreInventoryItem item;
    [SerializeField] private RawImage oreImage;


    public void Setup()
    {
        oreImage.color = NetworkClient.localPlayer.GetComponent<GamePlayer>().team == Team.Red ? Color.blue : Color.red;
    }
}
