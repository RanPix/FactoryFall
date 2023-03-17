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
        [SerializeField] private Toggle isShowingTipsToggle;
        [SerializeField] private Toggle isShowingNickNamesToggle;
        [SerializeField] private Slider sensetivitySlider;
        [SerializeField] private Slider FOVSlider;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider playerSoundsVolumeSlider;
        [SerializeField] private Slider shootVolumeSlider;
        [SerializeField] private ArrayElementSelector graphicsQualitySelector;
        [SerializeField] private ArrayElementSelector healthBarColorSelector;

        [Space]

        [SerializeField] public AudioMixer MasterMixer;

        public static bool isShowingTips = true;
        public void UpdateisShowingGuideValue()
        {
            isShowingTips = isShowingTipsToggle.isOn;

            if (TipsManager.instance != null)
                TipsManager.instance.tipsIsActive = isShowingTips;
        }


        public static float sensetivity = 15;
        public void UpdateSensetivityValue()
        {
            sensetivity = sensetivitySlider.value;
        }


        public static float FOV = 60;
        public void UpdateFOVValue()
        {
            FOV = FOVSlider.value;
            if (NetworkClient.localPlayer != null)
            {
                NetworkClient.localPlayer.GetComponent<GamePlayer>().cameraHolder.GetComponent<Look>().UpdateFOV();
                NetworkClient.localPlayer.GetComponent<GamePlayer>().spectatorCamera.GetComponent<Look>().UpdateFOV();
            }
        }

        public static int graphicsQuality = 4;
        public void UpdateGraphicsQualityValue()
        {
            graphicsQuality = graphicsQualitySelector.currentElement;

            UpdateGraphicsQuality();
        }
        public static void UpdateGraphicsQuality()
            => QualitySettings.SetQualityLevel(graphicsQuality);


        public static float masterVolume = 0.2f;
        public const string masterMixerVolumeKey = "MasterVolume";
        public const float masterVolumeMultiplier = 20;
        public void UpdateMasterVolumeValue()
        {
            masterVolume = masterVolumeSlider.value;

            MasterMixer.SetFloat(masterMixerVolumeKey, Mathf.Log10(masterVolume) * masterVolumeMultiplier);
        }


        public static float playerSoundsVolume = 1;
        public const string playerSoundsMixerVolumeKey = "PlayerSoundsVolume";
        public const float playerSoundsVolumeMultiplier = masterVolumeMultiplier;
        public void UpdatePlayerSoundsVolumeValue()
        {
            playerSoundsVolume = playerSoundsVolumeSlider.value;

            MasterMixer.SetFloat(playerSoundsMixerVolumeKey, Mathf.Log10(playerSoundsVolume) * playerSoundsVolumeMultiplier);
        }


        public static float shootVolume = 1;
        public const string shootMixerVolumeKey = "ShootVolume";
        public const float shootVolumeMultiplier = masterVolumeMultiplier;
        public void UpdateShootVolumeValue()
        {
            shootVolume = shootVolumeSlider.value;

            MasterMixer.SetFloat(shootMixerVolumeKey, Mathf.Log10(shootVolume) * shootVolumeMultiplier);
        }


        public static int healthBarColor = 0;
        public void UpdatehealthBarColor()
        {
            healthBarColor = healthBarColorSelector.currentElement;
            if (NetworkClient.localPlayer != null)
            {
                GamePlayer gamePlayer = NetworkClient.localPlayer.GetComponent<GamePlayer>();
                gamePlayer.healthBar.GetComponent<HealthBar>().UpdateColor();
            }
        }


        public static bool isShowingNicknames = true;
        public void UpdateisShowingNickNamesValue()
        {
            isShowingNicknames = isShowingNickNamesToggle.isOn;

            if (NetworkClient.localPlayer != null)
            {
                NetworkClient.localPlayer.GetComponent<GamePlayer>().cameraHolder.GetComponent<Look>().UpdateShowingNickNames();
                NetworkClient.localPlayer.GetComponent<GamePlayer>().spectatorCamera.GetComponent<Look>().UpdateShowingNickNames();
            }
        }

        private void Awake()
        {
            isShowingTipsToggle.isOn = isShowingTips;
            sensetivitySlider.value = sensetivity;
            FOVSlider.value = FOV;
            masterVolumeSlider.value = masterVolume;
            playerSoundsVolumeSlider.value = playerSoundsVolume;
            shootVolumeSlider.value = shootVolume;
            graphicsQualitySelector.currentElement = graphicsQuality;
            healthBarColorSelector.currentElement = healthBarColor;
            isShowingNickNamesToggle.isOn = isShowingNicknames;
            UpdatehealthBarColor();
        }
    }
}
