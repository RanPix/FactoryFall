using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;

namespace PlayerSettings
{
    public class SettingsLoader : MonoBehaviour
    {
        static public string SettingsGameFilePath { get; private set; }

        [SerializeField] AudioMixer MasterMixer;

        void Awake()
        {
            SettingsGameFilePath = Application.persistentDataPath + "/SettingsData.txt";
            Debug.Log(SettingsGameFilePath);
            if (File.Exists(SettingsGameFilePath))
            {
                StreamReader fileR = new StreamReader(SettingsGameFilePath);
                Settings.isShowingTips = bool.Parse(fileR.ReadLine());
                Settings.isShowingNickNames = bool.Parse(fileR.ReadLine());
                Settings.sensetivity = float.Parse(fileR.ReadLine());
                Settings.FOV = float.Parse(fileR.ReadLine());
                Settings.masterVolume = float.Parse(fileR.ReadLine());
                Settings.playerSoundsVolume = float.Parse(fileR.ReadLine());
                Settings.shootVolume = float.Parse(fileR.ReadLine());
                Settings.graphicsQuality = int.Parse(fileR.ReadLine());
                Settings.healthBarColor = int.Parse(fileR.ReadLine());
            }
            else
            {
                Debug.LogError("No settings file. To fix it reenter the game. If it still doesnt work, kick serhiikos fukin ass cuz he's stupid dumbass asshole");
            }
            Settings.UpdateGraphicsQuality();

            MasterMixer.SetFloat(Settings.masterMixerVolumeKey, Mathf.Log10(Settings.masterVolume) * Settings.masterVolumeMultiplier);
            MasterMixer.SetFloat(Settings.playerSoundsMixerVolumeKey, Mathf.Log10(Settings.playerSoundsVolume) * Settings.playerSoundsVolumeMultiplier);
            MasterMixer.SetFloat(Settings.shootMixerVolumeKey, Mathf.Log10(Settings.shootVolume) * Settings.shootVolumeMultiplier);

            Destroy(gameObject);
        }
    }
}