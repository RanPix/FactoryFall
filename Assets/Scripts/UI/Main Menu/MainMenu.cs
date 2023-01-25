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
    ConnectToSomeoneOrHost = 1,
    Host = 2,
    ConnectToSomeone = 3,
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] NetworkManager manager;

    ButtonGroup currentButtonGroup = ButtonGroup.Main;
    [SerializeField] GameObject[] MainMenuButtonGroupsToGroupGameObject;
    [SerializeField] float buttonGroupMoveTime;

    [SerializeField] RectTransform closedGroupPosition;
    [SerializeField] RectTransform openedGroupPosition;

    [SerializeField] TMP_Text IPAdressInputFieldText;
    [SerializeField] TMP_Text passWordClientInputFieldText;
    [SerializeField] TMP_Text passWordHostInputFieldText;
    [SerializeField] TMP_Text nicknameInputFieldText;

    void Start()
    {
        SetMainButtonGroup();
    }

    public void Join()
    {
        string adress = IPAdressInputFieldText.text;
        Debug.Log(adress);
        manager.networkAddress = adress;
        manager.StartClient();
    }

    public void Host()
    {
        manager.StartHost();
    }

    public void Quit()
        =>Application.Quit();

    public void SetMainButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.Main);
    public void SetConnectToSomeoneOrHostButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.ConnectToSomeoneOrHost);
    public void SetHostButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.Host);
    public void SetConnectToSomeoneButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.ConnectToSomeone);

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
/*void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            // client ready
            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    NetworkClient.Ready();
                    if (NetworkClient.localPlayer == null)
                    {
                        NetworkClient.AddPlayer();
                    }
                }
            }

            StopButtons();

            GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("Host (Server + Client)"))
                    {
                        manager.StartHost();
                    }
                }

                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client"))
                {
                    manager.StartClient();
                }
                // This updates networkAddress every frame from the TextField
                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                GUILayout.EndHorizontal();

                // Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                else
                {
                    if (GUILayout.Button("Server Only")) manager.StartServer();
                }
            }
            else
            {
                // Connecting
                GUILayout.Label($"Connecting to {manager.networkAddress}..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    manager.StopClient();
                }
            }
        }

        void StatusLabels()
        {
            // host mode
            // display separately because this always confused people:
            //   Server: ...
            //   Client: ...
            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");
            }
            // server only
            else if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
            }
            // client only
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
            }
        }

        void StopButtons()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Host"))
                {
                    manager.StopHost();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client"))
                {
                    manager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    manager.StopServer();
                }
            }
        }*/