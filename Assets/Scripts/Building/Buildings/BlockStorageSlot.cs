using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class BlockStorageSlot : MonoBehaviour
{
    [SerializeField] GameObject inventoryObject;

    Item item;

    [SerializeField] bool isFiltering;
    ItemType filteringType;

    public void PutItem()
    {
        ItemType puttingItemType = inventoryObject.GetComponent<CursorInventory>().item.itemType;
        if (isFiltering)
            if (puttingItemType != ItemType.Nothing && puttingItemType != filteringType)
                return;
        item = inventoryObject.GetComponent<CursorInventory>().item.PutItem(item);
    }
}
