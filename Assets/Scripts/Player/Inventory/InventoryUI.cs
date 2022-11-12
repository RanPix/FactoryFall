using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private PlayerControls controls;

    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private GameObject inventoryPanelPrefab;
    [SerializeField] private GameObject inventoryPanelSlotPrefab;

    public bool isPanelOpened = false;

    const int slotsXOffset = 45;
    const int slotsYOffset = 500;
    const int distanceBetweenSlots = 5;
    const int slotSize = 80;
    const int slotsInRow = 9;

    public void ReloadInventoryPanel()//не чіпайте, будь ласка
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        int slotsCounts = gameObject.GetComponent<Inventory>().slotsCount;
        for (int i = 0; i < slotsCounts; i++)
        {
            int x = slotsXOffset + (i % slotsInRow * (distanceBetweenSlots + slotSize));
            int y = slotsYOffset - (i / slotsInRow * (distanceBetweenSlots + slotSize));

            GameObject instantiatedSlot = Instantiate(inventoryPanelSlotPrefab, new Vector3(x, y, 0), new Quaternion(), inventoryPanel.transform);
            instantiatedSlot.GetComponent<InventorySlotButton>().inventoryObject = gameObject;
            instantiatedSlot.GetComponent<InventorySlotButton>().slotIndex = i;
            instantiatedSlot.GetComponent<InventorySlotButton>().ReloadButton();
        }
        inventoryPanel.SetActive(isPanelOpened);
    }

    public void OpenOrClosePanel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

            isPanelOpened = !isPanelOpened;
            ReloadInventoryPanel();
        }
    }

    void Start()
    {
        controls = new PlayerControls();
        controls.UI.Enable();
        controls.UI.OpenOrCloseInventory.performed += OpenOrClosePanel;
        ReloadInventoryPanel();
    }
}
