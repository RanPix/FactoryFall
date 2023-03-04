using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PlayerSettings
{
    public class SettingsLoader : MonoBehaviour
    {
        [SerializeField] AudioMixer MasterMixer;

        void Awake()
        {
            if (PlayerPrefs.HasKey(Settings.sensetivityPrefsKey))
            {
                Settings.isShowingGuide = PlayerPrefs.GetInt(Settings.isShowingGuidePrefsKey) == 1;
                Settings.sensetivity = PlayerPrefs.GetFloat(Settings.sensetivityPrefsKey);
                Settings.FOV = PlayerPrefs.GetFloat(Settings.FOVPrefsKey);
                Settings.graphicsQuality = PlayerPrefs.GetInt(Settings.graphicsQualityPrefsKey);
                Settings.healthBarColor = PlayerPrefs.GetInt(Settings.healthBarColorPrefsKey);
                Settings.masterVolume = PlayerPrefs.GetFloat(Settings.masterVolumePrefsKey);
                Settings.playerSoundsVolume = PlayerPrefs.GetFloat(Settings.playerSoundsVolumePrefsKey);
                Settings.shootVolume = PlayerPrefs.GetFloat(Settings.shootVolumePrefsKey);
            }

            Settings.UpdateGraphicsQuality();

            MasterMixer.SetFloat(Settings.masterMixerVolumeKey, Mathf.Log10(PlayerPrefs.GetFloat(Settings.masterVolumePrefsKey)) * Settings.masterVolumeMultiplier);
            MasterMixer.SetFloat(Settings.playerSoundsMixerVolumeKey, Mathf.Log10(PlayerPrefs.GetFloat(Settings.playerSoundsVolumePrefsKey)) * Settings.playerSoundsVolumeMultiplier);
            MasterMixer.SetFloat(Settings.shootMixerVolumeKey, Mathf.Log10(PlayerPrefs.GetFloat(Settings.shootVolumePrefsKey)) * Settings.shootVolumeMultiplier);

            Destroy(gameObject);
        }
    }
}