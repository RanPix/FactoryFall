using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotButton : MonoBehaviour
{
    public int slotIndex;
    public GameObject inventoryObject;
    [SerializeField] private GameObject ItemCountTextGameObject;
    [SerializeField] private GameObject ItemImageGameObject;

    public void OnClick()
    {
        Item itemToPut = inventoryObject.GetComponent<CursorInventory>().item;
        inventoryObject.GetComponent<CursorInventory>().item = inventoryObject.GetComponent<Inventory>().items[slotIndex].PutItem(itemToPut);
        //Debug.Log(inventoryObject.GetComponent<CursorInventory>().item);
        ReloadButton();
    }

    public void ReloadButton()
    {
        Inventory inventory = inventoryObject.GetComponent<Inventory>();
        ItemTypeInfo itemTypeInfo = FindObjectOfType<ItemTypeToScriptableObject>().GetItemTypeInfo(inventory.items[slotIndex].itemType);

        ItemImageGameObject.GetComponent<Image>().sprite = itemTypeInfo.icon;
        int itemCount = inventory.items[slotIndex].count;
        ItemCountTextGameObject.GetComponent<TMPro.TextMeshProUGUI>().text = itemCount > 1 ? itemCount.ToString() : "";
        
    }
}
