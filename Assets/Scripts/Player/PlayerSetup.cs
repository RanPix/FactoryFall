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
            if (player.team != Team.Null)
            {
                player.SetupPlayer();
            }
            else
            {
                player.OnSetPlayerInfoTransfer += player.SetupPlayer;
            }

            return;
        }

        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void OnDestroy()
    {
        player.OnSetPlayerInfoTransfer -= player.SetupPlayer;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        player = GetComponent<GamePlayer>();
        /*if (player.isLocalPlayer)
            return;*/
        GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString(), player);

    }
}
