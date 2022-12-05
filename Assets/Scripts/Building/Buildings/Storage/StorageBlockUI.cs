using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBlockUI : MonoBehaviour, IInteractable
{
    [Header("Prefabs")]

    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private GameObject slotPrefab;

    const int slotsXOffset = -355;
    static int slotsYOffset = 400;
    const int distanceBetweenSlots = 5;
    const int slotSize = 80;
    const int slotsInRow = 9;

    static protected GameObject inventoryObject;
    protected GameObject panel;


    private bool isPanelOpened;

    public void RecreatePanel(GameObject inventoryObject)
    {
        if (panel != null)
            Destroy(panel);

        panel = Instantiate(panelPrefab, inventoryObject.GetComponent<InventoryUI>().canvas);
        RecreateSlots();
        ReloadSlots();
    }

    public void RecreateSlots()
    {
        foreach (GameObject slot in panel.transform)
            Destroy(slot);

        Slot[] slots = gameObject.GetComponent<StorageBlock>().slots;
        for (int i = 0; i < slots.Length; i++)
        {
            float x = slotsXOffset + (i % slotsInRow * (distanceBetweenSlots + slotSize));
            float y = slotsYOffset - (i / slotsInRow * (distanceBetweenSlots + slotSize));
            GameObject instantiatedSlot = Instantiate(slotPrefab, panel.transform);
            instantiatedSlot.transform.localPosition = new Vector3(x, y, 0);
            instantiatedSlot.GetComponent<StorageBlockSlotUI>().inventoryObject = inventoryObject;
            instantiatedSlot.GetComponent<StorageBlockSlotUI>().storageBlock = gameObject;
        }

        ReloadSlots();
    }

    public void ReloadSlots()
    {
        foreach (GameObject slot in panel.transform)
            slot.GetComponent<CraftingBlockInputSlotUI>().ReloadUI();
    }

    public void OpenOrClosePanel(GameObject inventoryObject)
    {
        isPanelOpened = !isPanelOpened;
        if (!isPanelOpened)
            Destroy(panel);
        else
            RecreatePanel(inventoryObject);
    }

    public void Interact(GameObject inventoryObject)
    {
        StorageBlockUI.inventoryObject = inventoryObject;
        OpenOrClosePanel(inventoryObject);
    }
}