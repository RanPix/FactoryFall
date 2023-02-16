using System;
using System.Collections;
using System.Linq;
using System.Text;
using Mirror;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainChat : MonoBehaviour
{
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
    public bool isWriting = false;

    private PlayerControls controls;


    // Start is called before the first frame update
    void Start()
    {
        view.SetActive(false);
        isOpened = false;

        controls = new PlayerControls();
        controls.UI.Enable();

        controls.UI.OpenChat.performed += OpenChat;
        controls.UI.OpenChat.performed += SendMessage;
        controls.UI.OpenOrCloseMenu.performed += CloseChat;

        group.padding.top = 513;
    }

    // Update is called once per frame
    void Update()
    {
        if(!look)
            look = NetworkClient.localPlayer?.gameObject.GetComponent<GamePlayer>().cameraHolder.GetComponent<Look>();
    }


    public void SetWritingState() => isWriting = true;
    

    public void OpenChat(InputAction.CallbackContext context)
    {
        if(isOpened)
            return;

        CursorManager.SetCursorLockState(CursorLockMode.None);
        CursorManager.disablesToLockCount++;
        look.canRotateCamera = false;
        view.SetActive(true);


        isOpened = true;
    }


    public void CloseChat(InputAction.CallbackContext context)
    {
        if(!isOpened)
            return;

        CursorManager.disablesToLockCount--;
        look.canRotateCamera = true;

        view.SetActive(false);

        isWriting = false;
        isOpened = false;
    }

    public IEnumerator AddItem(string name, string message, string senderColor)
    {
        GameObject _charItem = Instantiate(chatItem, content);
        _charItem.GetComponentInChildren<TMP_Text>().text = $"<size=30><b><color=#{senderColor}>{name}</color></b></size>:<color=#ff000000>_</color><size=20>{message}</size>";
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
        if (!isWriting)
            return;
        inputField.text = CheckMessage();
        teamColor.GetTeamColor(NetworkClient.localPlayer.GetComponent<GamePlayer>().team);
        ChatManager.instance.AddElement(NetworkClient.localPlayer.GetComponent<GamePlayer>().nickname, inputField.text,  teamColor.ToHexString());

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
