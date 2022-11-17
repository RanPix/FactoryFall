using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryObject;
    [SerializeField] GameObject ItemImageGameObject;
    [SerializeField] GameObject ItemCountTextGameObject;

    Item item;

    public int slotIndex;
    void Awake()
    {
        ReloadUI();
    }

    public void OnClick()
    {
        
    }

    public void ReloadUI()
    {
        if (!item.IsNull())
        {
            ItemTypeInfo itemTypeInfo = FindObjectOfType<ItemTypeToScriptableObject>().GetItemTypeInfo(item.itemType);
            ItemImageGameObject.GetComponent<Image>().sprite = itemTypeInfo.icon;
            ItemCountTextGameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.count > 1 ? item.count.ToString() : "";
        }
    }
}
