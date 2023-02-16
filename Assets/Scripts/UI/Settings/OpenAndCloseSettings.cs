using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAndCloseSettings : MonoBehaviour
{
    [SerializeField] GameObject settingsPrefab;


    public GameObject settings { get; private set; }

    public bool isOpened { get; private set; } = false;

    public void OpenSettings()
    {
        if (!isOpened)
        {
            settings = Instantiate(settingsPrefab);
        }
    }

    public void CloseSettings()
    {
        if (isOpened)
        {

        }
    }
}
