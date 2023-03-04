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
        [SerializeField] private Slider playerSoundsVolumeSlider;
        [SerializeField] private Slider shootVolumeSlider;
        [SerializeField] private Toggle isShowingGuideToggle;
        [SerializeField] private ArrayElementSelector graphicsQualitySelector;
        [SerializeField] private ArrayElementSelector healthBarColorSelector;

        [Space]

        [SerializeField] public AudioMixer MasterMixer;

        public static bool isShowingGuide = true;
        public const string isShowingGuidePrefsKey = "isShowingGuide";
        public void UpdateisShowingGuideValue()
        {
            isShowingGuide = isShowingGuideToggle.isOn;
            PlayerPrefs.SetInt(isShowingGuidePrefsKey, isShowingGuide? 1 : 0);

            if (TipsManager.instance != null)
                TipsManager.instance.tipsIsActive = isShowingGuide;
        }


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
        public void UpdateGraphicsQualityValue()
        {
            graphicsQuality = graphicsQualitySelector.currentElement;
            PlayerPrefs.SetInt(graphicsQualityPrefsKey, graphicsQuality);

            Debug.Log("asssaaaaaaaaaaaaaaaaaaaaaaaaaasasasas");

            UpdateGraphicsQuality();
        }
        public static void UpdateGraphicsQuality()
            => QualitySettings.SetQualityLevel(graphicsQuality);


        public static float masterVolume = 1;
        public const string masterVolumePrefsKey = "masterVolume";
        public const string masterMixerVolumeKey = "MasterVolume";
        public const float masterVolumeMultiplier = 20;
        public void UpdateMasterVolumeValue()
        {
            masterVolume = masterVolumeSlider.value;
            PlayerPrefs.SetFloat(masterVolumePrefsKey, masterVolume);

            MasterMixer.SetFloat(masterMixerVolumeKey, Mathf.Log10(masterVolume) * masterVolumeMultiplier);
        }


        public static float playerSoundsVolume = 1;
        public const string playerSoundsVolumePrefsKey = "playerSoundsVolume";
        public const string playerSoundsMixerVolumeKey = "PlayerSoundsVolume";
        public const float playerSoundsVolumeMultiplier = masterVolumeMultiplier;
        public void UpdatePlayerSoundsVolumeValue()
        {
            playerSoundsVolume = playerSoundsVolumeSlider.value;
            PlayerPrefs.SetFloat(playerSoundsVolumePrefsKey, playerSoundsVolume);

            MasterMixer.SetFloat(playerSoundsMixerVolumeKey, Mathf.Log10(playerSoundsVolume) * playerSoundsVolumeMultiplier);
        }


        public static float shootVolume = 1;
        public const string shootVolumePrefsKey = "shootVolume";
        public const string shootMixerVolumeKey = "ShootVolume";
        public const float shootVolumeMultiplier = masterVolumeMultiplier;
        public void UpdateShootVolumeValue()
        {
            shootVolume = shootVolumeSlider.value;
            PlayerPrefs.SetFloat(shootVolumePrefsKey, shootVolume);

            MasterMixer.SetFloat(shootMixerVolumeKey, Mathf.Log10(shootVolume) * shootVolumeMultiplier);
        }


        public static int healthBarColor = 0;
        public const string healthBarColorPrefsKey = "healthBarColor";
        public void UpdatehealthBarColor()
        {
            healthBarColor = healthBarColorSelector.currentElement;
            PlayerPrefs.SetInt(healthBarColorPrefsKey, healthBarColor);
            if (NetworkClient.localPlayer != null)
            {
                GamePlayer gamePlayer = NetworkClient.localPlayer.GetComponent<GamePlayer>();
                gamePlayer.healthBar.GetComponent<HealthBar>().UpdateColor();
            }
        }


        private void Awake()
        {
            isShowingGuideToggle.isOn = isShowingGuide;
            sensetivitySlider.value = sensetivity;
            FOVSlider.value = FOV;
            masterVolumeSlider.value = masterVolume;
            playerSoundsVolumeSlider.value = playerSoundsVolume;
            shootVolumeSlider.value = shootVolume;
            graphicsQualitySelector.currentElement = graphicsQuality;
            healthBarColorSelector.currentElement = healthBarColor;
            UpdatehealthBarColor();
        }
    }
}
