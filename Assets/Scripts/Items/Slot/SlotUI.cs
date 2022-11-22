using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemSystem;

public abstract class SlotUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryObject;
    [SerializeField] GameObject ItemImageGameObject;
    [SerializeField] GameObject ItemCountTextGameObject;

    void Awake()
    {
        ReloadUI();
    }

    public void OnClick()
    {
        Slot newSlotItem = GetSlot();
        Item cursorItem = inventoryObject.GetComponent<CursorInventory>().item;
        inventoryObject.GetComponent<CursorInventory>().item = newSlotItem.item.PutItem(cursorItem);
        SetSlot(newSlotItem);
        ReloadUI();
    }

    public abstract Slot GetSlot();
    public Item GetItem() => GetSlot().item;
    public abstract void SetSlot(Slot slot);

    public void ReloadUI()
    {
        Item item = GetSlot().item;

        if (!item.IsNull())
        {
            ItemTypeInfo itemTypeInfo = FindObjectOfType<ItemTypeToScriptableObject>().GetItemTypeInfo(item.itemType);
            ItemImageGameObject.GetComponent<Image>().sprite = itemTypeInfo.icon;
            ItemCountTextGameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.count > 1 ? item.count.ToString() : "";
        }
    }
}
