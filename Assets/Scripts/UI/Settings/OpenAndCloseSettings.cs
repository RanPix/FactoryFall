using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSettings
{
    public class OpenAndCloseSettings : MonoBehaviour
    {
        [SerializeField] GameObject settingsPrefab;
        static Transform canvas;


        private void Start()
        {
            canvas = GameObject.Find("Canvas").transform;
        }
        public GameObject settings { get; private set; }

        public bool isOpened { get; private set; } = false;

        public void OpenSettings()
        {
            if (!isOpened)
            {
                settings = Instantiate(settingsPrefab, canvas);
                settings.GetComponent<Settings>().closeButton.onClick.AddListener(CloseSettings);
                isOpened = true;
            }
        }

        public void CloseSettings()
        {
            if (isOpened)
            {
                Destroy(settings);
                isOpened = false;
            }
        }
    }
}
