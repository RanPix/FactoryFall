using UnityEngine;
using System.Collections;
using Mirror;

[RequireComponent(typeof(AudioSource))]
public class AudioSync : NetworkBehaviour
{
    private AudioSource source;

    public AudioClip[] clips;
    void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    public void PlaySound(int id)
    {
        if (id >= 0 && id < clips.Length)
        {
            CmdSendServerSoundID(id);
        }

    }

    [Command]
    private void CmdSendServerSoundID(int id)
    {
        RpcSendSoundIDToClients(id);
    }

    [ClientRpc]
    private void RpcSendSoundIDToClients(int id)
    {
        source.PlayOneShot(clips[id]);
    }
}

