using System;
using UnityEngine;
using Mirror;
using System.Collections.Generic;
using Player;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NetworkManagerFF : NetworkManager
{
    private static Team playersCurrentTeam = Team.Null;
    private static List<Transform> spawnPositions = new ();

    public Action<bool> onGameStop;

     

    public static Transform GetRespawnPosition(Team team)
    {
        if (team != playersCurrentTeam)
        {
            playersCurrentTeam = team;
            spawnPositions.Clear();

            foreach (Transform point in startPositions)
            {
                if (point.tag == $"{team}TeamSpawnPoint")
                {
                    spawnPositions.Add(point);
                }
            }
        }

        return spawnPositions[Random.Range(0, spawnPositions.Count - 1)];
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        GameManager.instance.CmdRemovePlayerFromAllClientsLists(conn.identity.netId.ToString());
        //GameManager.instance.CmdUnRegisterAllPlayers();

        base.OnServerDisconnect(conn);
    }


    public override void OnStopHost()
    {
        if (NetworkClient.active)
        {
            GameManager.instance?.CmdUnRegisterAllPlayers();
            GameManager.instance?.UnregisterAllPlayers();
        }
        playersCurrentTeam = Team.Null;

        PlayerInfoTransfer.instance.SetNullInstance();
        Destroy(PlayerInfoTransfer.instance.gameObject);

        onGameStop?.Invoke(false);

        SceneManager.UnloadSceneAsync("MAP_CageCastle");
        base.OnStopHost();
    }

    public override void OnStartHost()
    {
        SceneManager.UnloadSceneAsync("Main Menu");
        base.OnStartHost();
        
    }
    public override void OnStopClient()
    {
        GameManager.instance?.CmdRemovePlayerFromAllClientsLists(NetworkClient.localPlayer.GetComponent<GamePlayer>().netId.ToString());
        GameManager.instance?.UnregisterAllPlayers();
        playersCurrentTeam = Team.Null;

        PlayerInfoTransfer.instance.SetNullInstance();
        Destroy(PlayerInfoTransfer.instance.gameObject);

        onGameStop?.Invoke(false);

        SceneManager.UnloadSceneAsync("MAP_CageCastle");
        base.OnStopHost();
    }


    
}

