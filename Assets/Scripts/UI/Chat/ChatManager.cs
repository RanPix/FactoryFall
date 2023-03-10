using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddElement(string name, string message, string senderColor)
    {

        CmdAddElement(name, message, senderColor);
    }

    [Command(requiresAuthority = false)]
    public void CmdAddElement( string name, string message, string senderColor)
    {
        RpcAddElement( name, message, senderColor);
    }

    [ClientRpc]
    private void RpcAddElement(string name, string message, string senderColor)
    {
        StartCoroutine(CanvasInstance.instance.mainChat.AddItem(name, message, senderColor));
    }
}
