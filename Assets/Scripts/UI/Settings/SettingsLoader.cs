using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSettings
{
    public class SettingsLoader : MonoBehaviour
    {
        void Awake()
        {
            if (PlayerPrefs.HasKey(Settings.sensetivityPrefsKey))
            {
                Settings.sensetivity = PlayerPrefs.GetFloat(Settings.sensetivityPrefsKey);
                Settings.FOV = PlayerPrefs.GetFloat(Settings.FOVPrefsKey);

            }
            Destroy(gameObject);
        }
    }
}