using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Player;
using UnityEngine.Audio;

namespace PlayerSettings
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] public Button closeButton;
        [SerializeField] private Slider sensetivitySlider;
        [SerializeField] private Slider FOVSlider;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private ArrayElementSelector graphicsQualitySelector;

        [Space]

        [SerializeField] private AudioMixer MasterMixer;

        public static float sensetivity = 15;
        public const string sensetivityPrefsKey = "sensetivity";
        public void UpdateSensetivityValue()
        {
            sensetivity = sensetivitySlider.value;
            PlayerPrefs.SetFloat(sensetivityPrefsKey, sensetivity);
        }


        public static float FOV = 60;
        public const string FOVPrefsKey = "FOV";
        public void UpdateFOVValue()
        {
            FOV = FOVSlider.value;
            PlayerPrefs.SetFloat(FOVPrefsKey, FOV);
            if (NetworkClient.localPlayer != null)
            {
                NetworkClient.localPlayer.GetComponent<GamePlayer>().cameraHolder.GetComponent<Look>().UpdateFOV();
            }
        }


        public static int graphicsQuality = 2;
        public const string graphicsQualityPrefsKey = "graphicsQuality";
        public void UpdateGraphicsQuality()
        {
            graphicsQuality = graphicsQualitySelector.currentElement;
            PlayerPrefs.SetInt(graphicsQualityPrefsKey, graphicsQuality);

            switch(graphicsQuality)
            {
                case 0:
                    //??? ????? 
                    break;
                case 1:
                    QualitySettings.currentLevel = QualityLevel.Simple;
                    break;
                case 2:
                    QualitySettings.currentLevel = QualityLevel.Good;
                    break;
                case 3:
                    QualitySettings.currentLevel = QualityLevel.Fantastic;
                    break;
            };
        }


        public static float masterVolume = 1;
        public const string masterVolumePrefsKey = "masterVolume";
        public const string MixerVolumeKey = "MasterVolume";
        public const float masterVolumeMultiplier = 20;
        public void UpdateMasterVolumeValue()
        {
            masterVolume = masterVolumeSlider.value;
            PlayerPrefs.SetFloat(masterVolumePrefsKey, masterVolume);

            MasterMixer.SetFloat(MixerVolumeKey, Mathf.Log10(masterVolume) * masterVolumeMultiplier);
        }


        private void Awake()
        {
            sensetivitySlider.value = sensetivity;
            FOVSlider.value = FOV;
            masterVolumeSlider.value = masterVolume;
            graphicsQualitySelector.currentElement = graphicsQuality;
        }
    }
}
