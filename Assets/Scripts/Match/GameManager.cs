using System.Collections.Generic;
using System.Linq;
using System;
using Mirror;
using Player;
using Player.Info;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MatchSettings matchSettings;

    [SerializeField] private GameObject sceneCamera;

    public Action<string, string> OnPlayerKilledCallback;

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
        SetSceneCameraActive(false);
    }

    public void SetSceneCameraActive(bool state)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.SetActive(state);
    }

    #region Player tracking

    
    private static Dictionary<string, PlayerInfo> players = new Dictionary<string, PlayerInfo>();

    public static void RegisterPlayer(string _netID, PlayerInfo _player)
    {
        Debug.Log("NetID = " + _netID);
        players.Add(_netID, _player);
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static PlayerInfo GetPlayer(string _playerID)
    {
        Debug.Log("get player info = " + _playerID);
        return players[_playerID];
    }

    public static PlayerInfo[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    void OnGUI()
    {
        //return; // if you dont need this

        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string playerID in players.Keys)
            GUILayout.Label($"{playerID} - {players[playerID]}");

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    #endregion
}