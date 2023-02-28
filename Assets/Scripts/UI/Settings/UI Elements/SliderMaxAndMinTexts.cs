using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderMaxAndMinTexts : MonoBehaviour
{
    [SerializeField] Slider slider;

    [SerializeField] string minValue;//if no value, it sets texts automaticly
    [SerializeField] string maxValue;
    [SerializeField] TMP_Text minValueText;
    [SerializeField] TMP_Text MaxValueText;
    void Start()
    {
        minValueText.text = minValue == "" ? slider.minValue.ToString() : minValue;
        MaxValueText.text = maxValue == "" ? slider.maxValue.ToString() : maxValue;
    }
}
