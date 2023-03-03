using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using GameBase;
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
        [SerializeField] private ArrayElementSelector healthBarColorSelector;

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

            QualitySettings.SetQualityLevel(graphicsQuality);
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


        public static int healthBarColor = 0;
        public const string healthBarColorPrefsKey = "healthBarColor";
        public void UpdatehealthBarColor()
        {
            healthBarColor = healthBarColorSelector.currentElement;
            PlayerPrefs.SetFloat(healthBarColorPrefsKey, healthBarColor);
            if (NetworkClient.localPlayer != null)
            {
                GamePlayer gamePlayer = NetworkClient.localPlayer.GetComponent<GamePlayer>();
                gamePlayer.healthBar.GetComponent<HealthBar>().UpdateColor();
            }
        }


        private void Awake()
        {
            sensetivitySlider.value = sensetivity;
            FOVSlider.value = FOV;
            masterVolumeSlider.value = masterVolume;
            graphicsQualitySelector.currentElement = graphicsQuality;
            healthBarColorSelector.currentElement = healthBarColor;
            UpdatehealthBarColor();
        }
    }
}
