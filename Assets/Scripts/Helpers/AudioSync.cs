using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FiniteMovementStateMachine;
using Mirror;

[System.Serializable]
public struct WeaponAudioEffects
{
    public string[] names;
    public AudioClip[] clips;

}
[System.Serializable]
public struct PlayerAudioEffects
{
    public string[] names;
    public AudioClip[] clips;

}
public enum ClipType
{
    weapon,
    player,
}
[RequireComponent(typeof(AudioSource))]
public class AudioSync : NetworkBehaviour
{
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private AudioSource weaponSource;

    [SerializeField] private WeaponAudioEffects weaponAudioEffects;
    [SerializeField] private PlayerAudioEffects playerAudioEffects;

    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    void Start()
    {
        if(weaponAudioEffects.clips.Length != weaponAudioEffects.names.Length)
            Debug.LogError("The number of weapon audio clips must be equal to the number of clips names");

        if (playerAudioEffects.clips.Length != playerAudioEffects.names.Length)
            Debug.LogError("The number of player audio clips must be equal to the number of player clips names");


        for (int i = 0; i < weaponAudioEffects.clips.Length; i++)
        {
            if (!weaponAudioEffects.names[i].Contains('_'))
                Debug.LogError($"The name of the clip at index {i} should look like: Pistol_Reload, Shotgun_Shoot etc.");
            audioClips.Add(weaponAudioEffects.names[i], weaponAudioEffects.clips[i]);
        }

        for (int i = 0; i < playerAudioEffects.clips.Length; i++)
        {
            if (!char.IsUpper(playerAudioEffects.names[i][0]))
                Debug.LogError($"The name of the clip under index {i} must be uppercase in the first index");
            audioClips.Add(playerAudioEffects.names[i], playerAudioEffects.clips[i]);
        }




        GetComponent<MovementMachine>().OnStateChange += PlayPlayerSound;
    }

    private void OnDestroy()
    {
        GetComponent<MovementMachine>().OnStateChange -= PlayPlayerSound;
    }

    private void PlayPlayerSound(string currentStateName)
    {

        currentStateName = char.ToUpper(currentStateName[0]) + currentStateName.Substring(1, currentStateName.Length - 1);
        if (!audioClips.Keys.Contains(currentStateName))
        {

            Debug.LogError( $"There is no player clip with name - {currentStateName}");
        }


        CmdSendServerSoundID(ClipType.player,false, currentStateName);
    }

    public void PlaySound(ClipType type, bool playOneShot, string soundName)
    {
        if(type == ClipType.weapon && !soundName.Contains('_'))
            Debug.LogError("The name of the clip should look like: Pistol_Reload, Shotgun_Shoot etc.");

        if (!audioClips.Keys.Contains(soundName))
        {
            Debug.LogError("There is no clip with that name");
        }
        CmdSendServerSoundID(type,playOneShot, soundName);

    }

    [Command]
    private void CmdSendServerSoundID(ClipType type, bool playOneShot, string clipName)
    {
        RpcSendSoundIDToClients(type,playOneShot, clipName);
    }

    [ClientRpc]
    private void RpcSendSoundIDToClients(ClipType type, bool playOneShot, string clipName)
    {
        switch (type)
        {
            case ClipType.player:
            {
                if (playOneShot)
                {
                    playerSource.PlayOneShot(audioClips[clipName]);
                }
                else
                {
                    playerSource.clip = audioClips[clipName];
                    playerSource.Play();
                }
                break;
            }
            case ClipType.weapon:
            {

                if (playOneShot)
                {
                    weaponSource.PlayOneShot(audioClips[clipName]);
                }
                else
                {
                    weaponSource.clip = audioClips[clipName];
                    weaponSource.Play();
                }

                break;
            }
        }
    }
}

