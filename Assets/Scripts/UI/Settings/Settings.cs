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


        public static float graphicsQuality = 15;
        public const string graphicsQualityPrefsKey = "graphicsQuality";
        public void UpdateGraphicsQuality()
        {
            /*FOV = FOVSlider.value;
            PlayerPrefs.SetFloat(FOVPrefsKey, FOV);
            if (NetworkClient.localPlayer != null)
            {
                NetworkClient.localPlayer.GetComponent<GamePlayer>().cameraHolder.GetComponent<Look>().UpdateFOV();
            }*/
        }


        public static float masterVolume = 60;
        public const string masterVolumePrefsKey = "masterVolume";
        public const float masterVolumeMultiplier = 20;
        public void UpdateMasterVolumeValue()
        {
            masterVolume = masterVolumeSlider.value;
            PlayerPrefs.SetFloat(masterVolumePrefsKey, masterVolume);

            MasterMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * masterVolumeMultiplier);
        }


        private void Awake()
        {
            sensetivitySlider.value = sensetivity;
            FOVSlider.value = FOV;
        }
    }
}
