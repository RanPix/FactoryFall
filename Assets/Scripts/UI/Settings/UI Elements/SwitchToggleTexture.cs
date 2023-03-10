using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggleTexture : MonoBehaviour
{
    [SerializeField] Sprite onTexture;
    [SerializeField] Sprite offTexture;
    [SerializeField] Image image;
    public void SetTexture(bool enabled)
    =>  image.sprite = enabled ? onTexture : offTexture;
}
