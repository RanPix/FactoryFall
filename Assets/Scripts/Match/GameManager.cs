using System.Collections.Generic;
using System.Linq;
using System;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    public MatchSettings matchSettings;
    public Action<bool> OnMatchSettingsSettedup;

    [SerializeField] private GameObject sceneCamera;

    [field: SyncVar] public bool gameEnded { get; private set; }


    public Action<string, Team, string, Team> OnPlayerKilledCallback;

    public Action OnClientStart;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }

        //matchSettings.Setup();

        //ChangeTeamSpawnPositions(PlayerInfoTransfer.instance.team);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        //print("yes");
        MatchSettingsTransfer transfer = MatchSettingsTransfer.instance;

        matchSettings.Setup(transfer.gm, transfer.hasTimer, transfer.matchTime, transfer.winningGoal);
    }

    public void SetSceneCameraActive(bool state)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.SetActive(state);
    }



    #region Player tracking

    
    private static Dictionary<string, GamePlayer> players = new Dictionary<string, GamePlayer>();

    public static void RegisterPlayer(string _netID, GamePlayer _player)
    {
        players.Add(_netID, _player);

        
        
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    [Server]
    public void CmdRemovePlayerFromAllClientsLists(string netID)
    {
        RpcRemovePlayerFromAllClientsLists(netID);
    }

    [ClientRpc]
    private void RpcRemovePlayerFromAllClientsLists(string netID)
    {
        UnRegisterPlayer(netID);
    }

    public static GamePlayer GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    public static GamePlayer[] GetAllPlayers()
    {
        
        return players.Values.ToArray();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnClientStart?.Invoke();
    }

    void OnGUI()
    {
        //return; // if you dont need this

        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string playerID in players.Keys)
        {
            GUILayout.Label($"{playerID} {players[playerID].team} - {players[playerID].nickname}, kills: {players[playerID].kills}, deaths: {players[playerID].deaths}");
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    #endregion


    [TargetRpc]
    public void TargetSetMatchSettings(NetworkConnection conn, MatchSettingsStruct settings)
    {
        matchSettings.SetMatchSettings(settings);
    }


    [Server]
    public void EndGame()
    {
        gameEnded = true;
        
        StartCoroutine(WaitForDisconnectPlayer());
    }

    [Server]
    private IEnumerator WaitForDisconnectPlayer()
    {
        yield return new WaitForSeconds(5f);

        NetworkServer.Shutdown();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        if (isServer)
            return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        NetworkManager.singleton.StopClient();

        players.Clear();

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene("Main Menu");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        if (isClient)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            NetworkManager.singleton.StopHost();
            //NetworkManager.singleton.StopClient();

            players.Clear();

            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadScene("Main Menu");
        }
    }

    public static Team GetBalancedTeam()
    {
        if (!instance.matchSettings.teamsMatch)
        {
            return Team.None;
        }

        GamePlayer[] playersList = GetAllPlayers();

        int blueAmount = 0;
        int redAmount = 0;

        foreach (GamePlayer player in playersList)
        {
            if (player.team == Team.Blue)
            {
                blueAmount++;
                continue;
            }

            if (player.team == Team.Red)
            {
                redAmount++;
                continue;
            }
        }

        if (blueAmount > redAmount)
            return Team.Blue;
        else if (redAmount > blueAmount)
            return Team.Red;
        else
            return Team.Blue;
    }
}