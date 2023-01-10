using UnityEngine;
using Mirror;
using Player;

[RequireComponent(typeof(GamePlayer))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] componentsToDisable;

    private GamePlayer player;

    [Client]
    private void Start()
    {
        if (isLocalPlayer)
        {
            player.SetupPlayer();

            return;
        }

        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();

        player = GetComponent<GamePlayer>();
        GameManager.RegisterPlayer(_netID, player.GetPlayerInfo());
    }
}
