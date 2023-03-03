using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipsManager : MonoBehaviour
{
    public static TipsManager instance;

    public string[] tipsMessages;
    public string[] tipsNames;

    [SerializeField] private TMP_Text tipText;

    public Dictionary<string, string> tips = new Dictionary<string, string>();
    public string currentTipName
    {
        get => _currentTipName; 
        set
        {
            _currentTipName = value;
            if(tipsIsActive)
                OnChangeTip?.Invoke(value);
        }
    }

    public bool tipsIsActive
    {
        get => _tipsIsActive;
        set
        {
            _tipsIsActive = value;
            OnSetActiveTips?.Invoke(value);
        }
    }

    public bool _tipsIsActive = true;

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
            tips.Add(tipsNames[i], tipsMessages[i]);
        }

        OnChangeTip += ChangeTip;
    }

    private void OnDestroy()
    {
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