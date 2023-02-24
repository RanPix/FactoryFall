using System;
using System.Collections;
using System.Linq;
using System.Text;
using FiniteMovementStateMachine;
using Mirror;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainChat : MonoBehaviour
{
    [SerializeField] private GamePlayer localPlayer;

    [SerializeField] private GameObject view;
    [SerializeField] private VerticalLayoutGroup group;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject chatItem;

    [SerializeField] private string[] banWords;

    private Look look;

    [SerializeField] private Transform content;
    [SerializeField] private TMP_Text massageInput;

    [SerializeField] private Color teamColor;

    public bool isOpened = false;

    public Action<bool> OnChatToggle;


    private PlayerControls controls;


    // Start is called before the first frame update
    void Start()
    {
        localPlayer = NetworkClient.localPlayer?.GetComponent<GamePlayer>();

        view.SetActive(false);
        isOpened = false;

        controls = new PlayerControls();
        controls.UI.Enable();

        controls.UI.OpenChat.performed += ChatToggle;
        controls.UI.OpenChat.performed += SendMessage;
        controls.UI.OpenOrCloseMenu.performed += CloseChat;

        group.padding.top = 513;
    }
    private void OnDestroy()
    {
        controls.UI.OpenChat.performed -= ChatToggle;
        controls.UI.OpenChat.performed -= SendMessage;
        controls.UI.OpenOrCloseMenu.performed -= CloseChat;


    }


    // Update is called once per frame
    void Update()
    {
        if(!look)
            look = NetworkClient.localPlayer?.gameObject.GetComponent<GamePlayer>().cameraHolder.GetComponent<Look>();
    }


    public void SelectInputField()
    {
        inputField.placeholder.GetComponent<TMP_Text>().text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void DeactivateBools()
    {
        if(!NetworkClient.localPlayer)
            return;

        if(!localPlayer)
            localPlayer = NetworkClient.localPlayer?.GetComponent<GamePlayer>();

        OnChatToggle?.Invoke(false);

        localPlayer.weaponKeyCodes.canShoot = false;

    }
    public void ActivateBools()
    {
        if(!NetworkClient.localPlayer)
            return;

        if(!localPlayer)
            localPlayer = NetworkClient.localPlayer?.GetComponent<GamePlayer>();


        OnChatToggle?.Invoke(true);

        localPlayer.weaponKeyCodes.canShoot = true;

    }

    public void ChatToggle(InputAction.CallbackContext context)
    {
        if (!isOpened)
        {
            DeactivateBools();
            CanvasInstance.instance.panelWithElementsToHide.transform.SetAsLastSibling();
            SelectInputField();

            //inputField.ActivateInputField();
            CursorManager.instance.SetCursorLockState(CursorLockMode.None);
            CursorManager.instance.disablesToLockCount++;
            look.canRotateCamera = false;
            view.SetActive(true);


            isOpened = true;

        }
        else
        {
            if (inputField.text.Replace(" ", "").Length > 0)
                return;

            CursorManager.instance.disablesToLockCount--;
            look.canRotateCamera = true;

            view.SetActive(false);

            isOpened = false;
            ActivateBools();

        }


    }


    public void CloseChat(InputAction.CallbackContext context)
    {
        if (inputField.text.Replace(" ", "").Length > 0)
            return;

        if (!isOpened)
            return;
        CursorManager.instance.disablesToLockCount--;
        look.canRotateCamera = true;

        view.SetActive(false);

        isOpened = false;
        ActivateBools();
    }

    public IEnumerator AddItem(string name, string message, string senderColor)
    {
        GameObject _charItem = Instantiate(chatItem, content);
        _charItem.GetComponentInChildren<TMP_Text>().text = $"<size=30><b><color=#{senderColor}>{name}</color></b></size>:<color=#ff000000>_</color><size=26>{message}</size>";
        yield return new WaitForNextFrameUnit();
        yield return new WaitForNextFrameUnit();

        _charItem.GetComponent<RectTransform>().sizeDelta = new Vector2(971.55f, (int)_charItem.GetComponentInChildren<TMP_Text>().GetComponent<RectTransform>().rect.height);
        _charItem.GetComponentInChildren<RectTransform>().position =
            new Vector3(_charItem.GetComponentInChildren<RectTransform>().position.x, 0, 0);

        _charItem.GetComponent<LayoutElement>().ignoreLayout = false;

        if (group.padding.top - (int)_charItem.GetComponentInChildren<TMP_Text>().GetComponent<RectTransform>().rect.height > 0)
            group.padding.top -= (int)_charItem.GetComponentInChildren<TMP_Text>().GetComponent<RectTransform>().rect.height;
        else
        {
            group.padding.top = 0;
        }


    }

    public void SendMessage(InputAction.CallbackContext context)
    {
        if (inputField.text.Replace(" ", "").Length < 1)
            return;
        print(inputField.text.Replace(" ", "").Length);
        print(inputField.text);
        print(inputField.text.Length);
        inputField.text = CheckMessage();
        teamColor.GetTeamColor(NetworkClient.localPlayer.GetComponent<GamePlayer>().team);
        ChatManager.instance.AddElement(NetworkClient.localPlayer.GetComponent<GamePlayer>().nickname, inputField.text,  teamColor.ToHexString());

        SelectInputField();
        inputField.text = "";
    }

    public string CheckMessage()
    {
        string[] words = inputField.text.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            if (banWords.Contains(words[i]))
            {
                string stars = "";
                for (int j = 0; j < words[i].Length; j++)
                {
                    if(words[i][j] == ' ')
                        stars+=" ";
                    else
                        stars += '*';
                }
                words[i] = stars;

            }

        }

        return String.Join(" ", words);
    }



}
