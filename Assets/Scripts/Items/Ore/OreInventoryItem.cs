using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OreInventoryItem : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    [field: Min(1)][field: SerializeField] public int maxCount { get; private set; }

    [field: Min(0), SerializeField]
    public int currentCount
    {
        get => _currentCount;
        set
        {
            _currentCount = value;
            OnCurrentCountchange?.Invoke();
        }
    }
    [SerializeField] private int  _currentCount;

    public Action OnCurrentCountchange;
    // Start is called before the first frame update
    void Awake()
    {
        UpdateCountText();
        OnCurrentCountchange += UpdateCountText;
    }

    private void Start()
    {
        OreGiveAwayArea.instance.OnAreaEnter += ResetCount;
    }
    private void ResetCount() => currentCount = 0;
    private void UpdateCountText()
    {
        text.text = $"{currentCount}/{maxCount}";
    }
}
