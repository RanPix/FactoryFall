using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderMaxAndMinTexts : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text minValueText;
    [SerializeField] TMP_Text MaxValueText;
    void Start()
    {
        minValueText.text = slider.minValue.ToString();
        MaxValueText.text = slider.maxValue.ToString();
    }
}
