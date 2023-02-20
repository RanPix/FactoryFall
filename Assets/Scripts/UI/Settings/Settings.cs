using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSettings
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] public Button closeButton;
        [SerializeField] public Slider sensetivitySlider;



        public static float sensetivity;
        public void UpdateSensetivityValue()
            => sensetivity = sensetivitySlider.value;

    }
}
