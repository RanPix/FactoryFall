using System;
using Mirror;
using PlayerSettings;
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
            if (CanvasInstance.instance.oreInventory.item.currentCount <= 0)
                return;
            OnAreaEnter?.Invoke(CanvasInstance.instance.oreInventory.item.currentCount);
            ////////////
            //CanvasInstance.instance.tipsManager.gameObject.SetActive(false);
            ///////////
            Settings.isShowingTips = false;
            CanvasInstance.instance.tipsManager.tipsIsActive = false;

            NetworkClient.localPlayer.GetComponent<AudioSync>().PlaySound(ClipType.player, true, "GiveAwayOre");
        }
    }

}
