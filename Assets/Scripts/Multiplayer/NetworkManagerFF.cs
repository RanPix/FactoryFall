using UnityEngine;
using Mirror;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Drawing;

public class NetworkManagerFF : NetworkManager
{
    private static Team playersCurrentTeam = Team.Null;
    private static List<Transform> spawnPositions = new List<Transform>();



    public override void Start()
    {
        base.Start();

        playersCurrentTeam = Team.Null;

        SceneManager.activeSceneChanged += GTFOTheMainMenu;
    }

    private void GTFOTheMainMenu(Scene oldScene, Scene newScene)
    {
        if (oldScene.name == "Main Menu")
        {
            SceneManager.UnloadSceneAsync(oldScene);
        }
    }

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

        base.OnServerDisconnect(conn);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);

        GameManager.instance.TargetSetMatchSettings(conn, GameManager.instance.matchSettings.matchSettingsStruct);
    }
}

