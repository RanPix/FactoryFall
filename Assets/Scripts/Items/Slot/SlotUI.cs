using UnityEngine;
using UnityEngine.UI;
using ItemSystem;

public abstract class SlotUI : MonoBehaviour
{
    public GameObject inventoryObject;
    [SerializeField] private GameObject ItemImageGameObject;
    [SerializeField] private GameObject ItemCountTextGameObject;

    void Start()
    {
        ReloadUI();
    }

    public void OnClick()
    {
        Slot SlotItem = GetSlot();
        Item CursorItem = inventoryObject.GetComponent<CursorInventory>().item;
        CursorItem = SlotItem.PutItem(CursorItem);
        inventoryObject.GetComponent<CursorInventory>().item = CursorItem;
        SetSlot(SlotItem);
        ReloadUI();
    }

    public abstract Slot GetSlot();

    public Item GetItem()
        => GetSlot().item;

    public abstract void SetSlot(Slot slot);

    public void ReloadUI()
    {
        Item item = GetItem();

        if (!item.IsNull())
        {
            ItemTypeInfo itemTypeInfo = FindObjectOfType<ItemTypeToScriptableObject>().GetItemTypeInfo(item.itemType);
            ItemImageGameObject.GetComponent<Image>().sprite = itemTypeInfo.Icon;
            ItemCountTextGameObject.GetComponent<TMPro.TextMeshProUGUI>().text = item.count > 1 ? item.count.ToString() : "";
        }
    }
}
