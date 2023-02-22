using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSettings
{
    public class SettingsLoader : MonoBehaviour
    {
        void Start()
        {
            if (PlayerPrefs.HasKey(Settings.sensetivityPrefsKey))
            {
                Settings.sensetivity = PlayerPrefs.GetFloat(Settings.sensetivityPrefsKey);
            }
            Destroy(gameObject);
        }
    }
}