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
        Item newSlotItem = GetItem();
        Item cursorItem = inventoryObject.GetComponent<CursorInventory>().item;
        inventoryObject.GetComponent<CursorInventory>().item = newSlotItem.PutItem(cursorItem);
        SetItem(newSlotItem);
        ReloadUI();
    }

    public abstract Item GetItem();
    public abstract Item SetItem(Item item);

    public void ReloadUI()
    {
        Item item = GetItem();

        if (!item.IsNull())
        {
            ItemTypeInfo itemTypeInfo = FindObjectOfType<ItemTypeToScriptableObject>().GetItemTypeInfo(item.itemType);
            ItemImageGameObject.GetComponent<Image>().sprite = itemTypeInfo.icon;
            ItemCountTextGameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.count > 1 ? item.count.ToString() : "";
        }
    }
}
