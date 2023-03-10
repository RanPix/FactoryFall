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
        if (GameManager.instance)
        {
            ULTRASETUP();
        }
        else
            GameManager.OnGameManagerSet += ULTRASETUP;

    }

    private void ULTRASETUP()
    {
        if (GameManager.instance.matchSettings.gm != Gamemode.BTR)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public void Setup()
        => oreImage.color = NetworkClient.localPlayer.GetComponent<GamePlayer>().team == Team.Red ? Color.blue : Color.red;
}
