using PlayerSettings;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipsManager : MonoBehaviour
{
    public static TipsManager instance;

    public string[] tipsMessages;
    public string[] tipsNames;

    [SerializeField] private TMP_Text tipText;
    [SerializeField] private Mask mask;

    public Dictionary<string, string> tips = new Dictionary<string, string>();
    public string currentTipName
    {
        get => _currentTipName; 
        set
        {
            _currentTipName = value;
            OnChangeTip?.Invoke(value);
        }
    }

    public bool tipsIsActive
    {
        get => _tipsIsActive;
        set
        {
            print($"value = {value}");
            mask.enabled = !value;
            _tipsIsActive = value;
            OnSetActiveTips?.Invoke(value);
        }
    }

    private bool _tipsIsActive = true;

    private string _currentTipName;

    public Action<string> OnChangeTip;
    public Action<bool> OnSetActiveTips;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }



        for (int i = 0; i < tipsMessages.Length; i++)
        {
            tipsMessages[i] = tipsMessages[i].Replace("\\n", "\n");
            tipsMessages[i] = tipsMessages[i].Replace("\\", "");

            tips.Add(tipsNames[i], tipsMessages[i]);
        }

        

        OnChangeTip += ChangeTip;

        tipsIsActive = Settings.isShowingTips;
    }

    private void OnDestroy()
    {
        OnChangeTip -= ChangeTip;
        instance = null;
    }
    public void ActivateTip(string tipName)
    {
        if(!tips.ContainsKey(tipName))
            Debug.LogError("There is no tip with this name");
        currentTipName = tipName;
    }
    private void ChangeTip(string newTipName)
    {
        tipText.text = tips[currentTipName];
    }

}
