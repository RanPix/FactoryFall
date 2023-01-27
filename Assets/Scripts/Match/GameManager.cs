using System.Collections.Generic;
using System.Linq;
using System;
using Mirror;
using Player;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] public static GameManager instance;

    public MatchSettings matchSettings;

    [SerializeField] private GameObject sceneCamera;

    public Action<string, string, int> OnPlayerKilledCallback;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }
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

    public static GamePlayer GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    public static GamePlayer[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    void OnGUI()
    {
        //return; // if you dont need this

        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string playerID in players.Keys)
            GUILayout.Label($"{playerID} - {players[playerID].name}");

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    #endregion
}