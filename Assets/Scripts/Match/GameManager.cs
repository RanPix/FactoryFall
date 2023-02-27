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

        matchSettings.Setup();

        //ChangeTeamSpawnPositions(PlayerInfoTransfer.instance.team);
    }

    private void OnDestroy()
    {
        print("inst");
        instance = null;
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

    public void UnregisterAllPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players.Remove(players.Keys.ToArray()[i]);
        }

    }

    [Command(requiresAuthority = false)]
    public void CmdUnRegisterAllPlayers()
    {
        RpcUnRegisterAllPlayers();
    }

    [ClientRpc]
    public void RpcUnRegisterAllPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players.Remove(players.Keys.ToArray()[i]);
        }
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

    //void OnGUI()
    //{
    //    //return; // if you dont need this

    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();

    //    foreach (string playerID in players.Keys)
    //    {
    //        GUILayout.Label($"{playerID} {players[playerID].team} - {players[playerID].nickname}, kills: {players[playerID].kills}, deaths: {players[playerID].deaths}");
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}
    #endregion

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

        //SceneManager.UnloadSceneAsync();
        SceneManager.UnloadSceneAsync("MAP_CageCastle");
        SceneManager.LoadScene("Main menu");
        NetworkManager.singleton.StopClient();
        base.OnStopClient();
    }

    public override void OnStopServer()
    {

        if (isClient)
        {
            //SceneManager.UnloadSceneAsync();
            SceneManager.UnloadSceneAsync("MAP_CageCastle");
            SceneManager.LoadScene("Main menu");
        }
        NetworkManager.singleton.StopHost();
        base.OnStopServer();
    }

}