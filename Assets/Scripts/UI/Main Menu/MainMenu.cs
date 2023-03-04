using UnityEngine;
using Mirror;
using DG.Tweening;
using TMPro;

public enum ButtonGroup
{
    Main = 0,
    JoinOrHost = 1,
    Join = 2,
}

public class MainMenu : MonoBehaviour
{
    ButtonGroup currentButtonGroup = ButtonGroup.Main;
    [SerializeField] GameObject[] MainMenuButtonGroupsToGroupGameObject;
    [SerializeField] float buttonGroupMoveTime;

    [SerializeField] RectTransform closedGroupPosition;
    [SerializeField] RectTransform openedGroupPosition;

    [SerializeField] TMP_InputField IPAdressInputFieldText;

    private void Start()
    {
        SetMainButtonGroup();
    }

    public void Join()
    {
        string adress = IPAdressInputFieldText.text;
        NetworkManager.singleton.networkAddress = adress;
        NetworkManager.singleton.StartClient();
    }

    public void Host()
    {
        NetworkManager.singleton.StartHost();
    }

    public void Quit()
        =>Application.Quit();

    public void SetMainButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.Main);
    public void SetConnectToSomeoneOrHostButtonGroup()
        => SetMenuButtonGroup(ButtonGroup.JoinOrHost);
    public void SetConnectToSomeoneButtonGroup()
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