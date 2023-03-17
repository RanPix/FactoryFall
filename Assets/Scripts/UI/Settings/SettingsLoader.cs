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
                StreamReader fileReader = new StreamReader(SettingsGameFilePath);
                Settings.isShowingTips = bool.Parse(fileReader.ReadLine());
                Settings.isShowingNicknames = bool.Parse(fileReader.ReadLine());
                Settings.sensetivity = float.Parse(fileReader.ReadLine());
                Settings.FOV = float.Parse(fileReader.ReadLine());
                Settings.masterVolume = float.Parse(fileReader.ReadLine());
                Settings.playerSoundsVolume = float.Parse(fileReader.ReadLine());
                Settings.shootVolume = float.Parse(fileReader.ReadLine());
                Settings.graphicsQuality = int.Parse(fileReader.ReadLine());
                Settings.healthBarColor = int.Parse(fileReader.ReadLine());
            }
            else
            {
                StreamWriter fileWriter = new StreamWriter(SettingsLoader.SettingsGameFilePath);
                fileWriter.WriteLine(Settings.isShowingTips);
                fileWriter.WriteLine(Settings.isShowingNicknames);
                fileWriter.WriteLine(Settings.sensetivity);
                fileWriter.WriteLine(Settings.FOV);
                fileWriter.WriteLine(Settings.masterVolume);
                fileWriter.WriteLine(Settings.playerSoundsVolume);
                fileWriter.WriteLine(Settings.shootVolume);
                fileWriter.WriteLine(Settings.graphicsQuality);
                fileWriter.WriteLine(Settings.healthBarColor);
            }
            Settings.UpdateGraphicsQuality();

            MasterMixer.SetFloat(Settings.masterMixerVolumeKey, Mathf.Log10(Settings.masterVolume) * Settings.masterVolumeMultiplier);
            MasterMixer.SetFloat(Settings.playerSoundsMixerVolumeKey, Mathf.Log10(Settings.playerSoundsVolume) * Settings.playerSoundsVolumeMultiplier);
            MasterMixer.SetFloat(Settings.shootMixerVolumeKey, Mathf.Log10(Settings.shootVolume) * Settings.shootVolumeMultiplier);

            Destroy(gameObject);
        }
    }
}