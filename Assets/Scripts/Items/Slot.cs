using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class Slot
{
    [Header("Main")]

    [SerializeField] private int slotIndex;
    [SerializeField] public Item item;
    [SerializeField] private GameObject inventoryObject;

    [Header("Filtering")]

    [SerializeField] private bool isFiltering;
    [SerializeField] public ItemType filteringType;

    public void PutItem()
    {
        Item itemToPut = inventoryObject.GetComponent<CursorInventory>().item;

        if (isFiltering)
            if (itemToPut.itemType != ItemType.Nothing && itemToPut.itemType != filteringType)
                return;

        inventoryObject.GetComponent<CursorInventory>().item = item.PutItem(itemToPut);
    }
}