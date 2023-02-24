using System;
using TMPro;
using Mirror;
using UnityEngine;

public class OreInventoryItem : MonoBehaviour
{
    [SerializeField] private TMP_Text oreAmountText;

    [field: Min(1)][field: SerializeField] public int maxCount { get; private set; }


    [field: Min(0), SerializeField]
    public int currentCount
    {
        get => _currentCount;
        set
        {
            _currentCount = value;
            if(value != 0)
                NetworkClient.localPlayer.GetComponent<AudioSync>().PlaySound(ClipType.player, true, "TakeOre");
            OnCurrentCountchange?.Invoke();
        }
    }
    [SerializeField] private int _currentCount;

    public Action OnCurrentCountchange;


    private void Awake()
    {
        UpdateCountText();
        OnCurrentCountchange += UpdateCountText;
    }

    private void Start()
    {
        OreGiveAwayArea.instance.OnAreaEnter += ResetCount;
    }
    private void OnDestroy()
    {
        OnCurrentCountchange -= UpdateCountText;
        OreGiveAwayArea.instance.OnAreaEnter -= ResetCount;

    }



    private void ResetCount(int amount) 
        => currentCount = 0;
    private void UpdateCountText()
    {
        oreAmountText.text = $"{currentCount}/{maxCount}";
    }
}
