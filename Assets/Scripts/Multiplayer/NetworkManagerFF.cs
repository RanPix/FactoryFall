using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class NetworkManagerFF : NetworkManager
{
    private static Team playersCurrentTeam = Team.Null;
    private static List<Transform> spawnPositions = new ();


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
}

