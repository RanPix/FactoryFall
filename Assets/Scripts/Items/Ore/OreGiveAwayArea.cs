using System;
using UnityEngine;

public class OreGiveAwayArea : MonoBehaviour
{
    public static OreGiveAwayArea instance { get; private set; }
    public Action<int> OnAreaEnter;

    private void Awake()
    {
        if (instance == null)
            instance = this;

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "LocalPlayer")
        {
            OnAreaEnter?.Invoke(CanvasInstance.instance.oreInventory.item.currentCount /* встав сюдт к≥льк≥сть руди у гравц€ п≥д час  заходженн€ в область*/);
        }
    }

}
