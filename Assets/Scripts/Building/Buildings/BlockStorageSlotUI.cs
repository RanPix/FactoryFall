using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockStorageSlotUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryObject;
    
    [SerializeField] GameObject ItemImageGameObject;
    [SerializeField] GameObject ItemCountTextGameObject;

    public int slotIndex;
    void Awake()
    {
        ReloadUI();
    }

    public void OnClick()
    {
        BlockInventory blockInventory = gameObject.GetComponent<BlockInventory>();
        gameObject.GetComponent<BlockStorageSlot>().PutItem();
    }

    public void ReloadUI()
    {
        BlockInventory blockStorage = gameObject.GetComponent<BlockInventory>();
        if (blockStorage != null)
        {
            Item item = blockStorage.items[slotIndex];

            ItemTypeInfo itemTypeInfo = FindObjectOfType<ItemTypeToScriptableObject>().GetItemTypeInfo(item.itemType);
            ItemImageGameObject.GetComponent<Image>().sprite = itemTypeInfo.icon;
            ItemCountTextGameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.count > 1 ? item.count.ToString() : "";
        }
    }
}
