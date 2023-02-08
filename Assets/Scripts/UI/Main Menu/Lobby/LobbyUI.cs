using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{

    public static LobbyUI instance;

    [Header("Host Join")]
    [SerializeField] InputField joinMatchInput;
    [SerializeField] List<Selectable> lobbySelectables = new List<Selectable>();
    [SerializeField] Canvas lobbyCanvas;
    [SerializeField] Canvas searchCanvas;
    bool searching = false;

    [Header("Lobby")]
    [SerializeField] Transform UIPlayerParent;
    [SerializeField] GameObject PlayerIconPrefab;
    [SerializeField] Text matchIDText;
    [SerializeField] GameObject beginGameButton;

    GameObject localPlayerLobbyUI;

    void Start()
    {
        instance = this;
    }

    public void SetStartButtonActive(bool active)
    {
        beginGameButton.SetActive(active);
    }

    public void HostPublic()
    {
        lobbySelectables.ForEach(x => x.interactable = false);

        //PlayerNetwork.localPlayer.HostGame(true);
    }

    public void HostPrivate()
    {
        lobbySelectables.ForEach(x => x.interactable = false);

        //PlayerNetwork.localPlayer.HostGame(false);
    }

    public void HostSuccess(bool success, string matchID)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;

            if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
            localPlayerLobbyUI = SpawnPlayerUIPrefab(PlayerNetwork.localPlayer);
            matchIDText.text = matchID;
        }
        else
        {
            lobbySelectables.ForEach(x => x.interactable = true);
        }
    }

    public void Join()
    {
        lobbySelectables.ForEach(x => x.interactable = false);

        PlayerNetwork.localPlayer.JoinGame(joinMatchInput.text.ToUpper());
    }

    public void JoinSuccess(bool success, string matchID)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;

            if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
            localPlayerLobbyUI = SpawnPlayerUIPrefab(PlayerNetwork.localPlayer);
            matchIDText.text = matchID;
        }
        else
        {
            lobbySelectables.ForEach(x => x.interactable = true);
        }
    }

    public void DisconnectGame()
    {
        if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
        PlayerNetwork.localPlayer.DisconnectGame();

        lobbyCanvas.enabled = false;
        lobbySelectables.ForEach(x => x.interactable = true);
    }

    public GameObject SpawnPlayerUIPrefab(PlayerNetwork player)
    {
        GameObject newUIPlayer = Instantiate(PlayerIconPrefab, UIPlayerParent);
        newUIPlayer.GetComponent<PlayerIcon>().playerNetwork = player;
        newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);

        return newUIPlayer;
    }

    public void BeginGame()
    {
        PlayerNetwork.localPlayer.BeginGame();
    }

    public void SearchGame()
    {
        StartCoroutine(Searching());
    }

    public void CancelSearchGame()
    {
        searching = false;
    }

    public void SearchGameSuccess(bool success, string matchID)
    {
        if (success)
        {
            searchCanvas.enabled = false;
            searching = false;
            JoinSuccess(success, matchID);
        }
    }

    IEnumerator Searching()
    {
        searchCanvas.enabled = true;
        searching = true;

        float searchInterval = 1;
        float currentTime = 1;

        while (searching)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = searchInterval;
                PlayerNetwork.localPlayer.SearchGame();
            }
            yield return null;
        }
        searchCanvas.enabled = false;
    }

}