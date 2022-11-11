using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotButton : MonoBehaviour
{
    public int slotIndex;
    public GameObject inventoryObject;
    private Sprite sprite;

    public void OnClick()
    {
        Item iteToPut = inventoryObject.GetComponent<CursorInventory>().item;
        inventoryObject.GetComponent<CursorInventory>().item = inventoryObject.GetComponent<Inventory>().items[slotIndex].PutItem(iteToPut);
        ReloadButton();
    }

    public void ReloadButton()
    {
        Inventory inventory = inventoryObject.GetComponent<Inventory>();
        ItemTypeInfo itemTypeInfo = FindObjectOfType<ItemTypeToScriptableObject>().GetItemTypeInfo(inventory.items[slotIndex].itemType);

        gameObject.GetComponent<Image>().sprite = itemTypeInfo.icon;
        int itemCount = inventory.items[slotIndex].count;
        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = itemCount > 1 ? itemCount.ToString() : "";
    }
}
