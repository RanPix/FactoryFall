using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public enum ButtonGroup
{
    Main = 0,
    JoinOrHost = 1,
    Host = 2,
    Join = 3,
}

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public static string MyMatchGuid;

    [SerializeField] NetworkManager manager;

    [SerializeField] GameObject[] MainMenuButtonGroupsToGroupGameObject;
    [SerializeField] float buttonGroupMoveTime;

    [SerializeField] RectTransform closedGroupPosition;
    [SerializeField] RectTransform openedGroupPosition;

    [SerializeField] TMP_Text nicknameInputTextField;
    [SerializeField] TMP_Text GUidTextField;
    [SerializeField] TMP_Text GUidText;
    //[SerializeField] TMP_Text passWordClientInputTextField;
    //[SerializeField] TMP_Text passWordHostInputTextField;

    ButtonGroup currentButtonGroup = ButtonGroup.Main;
    PlayerNetwork localPlayer => PlayerNetwork.localPlayer;

    void Start()
    {
        instance = this;
        SetMainButtonGroup();
    }

    public void JoinLobby()
    {
        string guid = GUidTextField.text;
        MatchMaker.instance.JoinGame(guid, localPlayer, out localPlayer.playerIndex);
    }

    public void HostPrivateLobby()
    {//                               vvv ?? ??? ?????? ????????
        Guid guid = Guid.NewGuid();//trusy mami3 kupi
        PlayerNetwork.localPlayer.HostGame(false, guid.ToString());
    }

    public void StartLobbyGame()
    {
        PlayerNetwork.localPlayer.BeginGame();
    }

    public void Quit()
        => Application.Quit();

    public void HostButton() 
    { 
        SetMenuButtonGroup(ButtonGroup.Host);
        HostPrivateLobby();
    }
    public void SetMainButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.Main);
    public void SetJoinOrHostButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.JoinOrHost);
    public void SetJoinButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.Join);

    public void SetMenuButtonGroup(ButtonGroup buttonGroup)
    {
        currentButtonGroup = buttonGroup;
        UpdateButtonGroupsPosition();
    }

    public void UpdateButtonGroupsPosition()
    {
        for (int i = 0; i < MainMenuButtonGroupsToGroupGameObject.Length; i++)
            if (i != (int)currentButtonGroup)
                MainMenuButtonGroupsToGroupGameObject[i].transform.DOMove(closedGroupPosition.position, buttonGroupMoveTime);

        MainMenuButtonGroupsToGroupGameObject[(int)currentButtonGroup].transform.DOMove(openedGroupPosition.position, buttonGroupMoveTime);
    }
}

