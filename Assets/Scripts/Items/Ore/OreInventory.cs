using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Player;

public class OreInventory : MonoBehaviour
{
    public OreInventoryItem item;
    [SerializeField] private RawImage oreImage;

    private void Start()
    {
        /*if (NetworkClient.localPlayer)
        {
            print(1);
            TrySetup();
        }
        else
        {
            print(2);
            NetworkClient.OnConnectedEvent += TrySetup;
        }*/


    }

    private void TrySetup()
    {
        if (NetworkClient.localPlayer.GetComponent<GamePlayer>().team != Team.Null)
        {
            print(3);
            Setup();
        }
        else
        {
            print(4);
            NetworkClient.localPlayer.GetComponent<GamePlayer>().OnSetPlayerInfoTransfer += Setup;
        }
    }


    public void Setup()
    {
        oreImage.color = NetworkClient.localPlayer.GetComponent<GamePlayer>().team == Team.Red ? Color.blue : Color.red;
        print(5);
    }
}
