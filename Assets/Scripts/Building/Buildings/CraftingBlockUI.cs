using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public abstract class CraftingBlockUI : MonoBehaviour, IInteractable
{
    [Header("Prefabs")]
    
    [SerializeField] GameObject panelPrefab;
    [SerializeField] GameObject inputSlotPrefab;
    [SerializeField] GameObject outputSlotPrefab;
    
    [Header("Slots Positioning Info")]

    [SerializeField] static int slotSize;
    [SerializeField] static int distanceBetweenSlots;
    [SerializeField] static int inputSlotsXOffset;
    [SerializeField] static int inputSlotsYOffset;
    [SerializeField] static int outputSlotsXOffset;
    [SerializeField] static int outputSlotsYOffset;

    protected GameObject canvas;
    protected GameObject panel;
    protected Transform inputSlotsTransform;
    protected Transform outputSlotsTransform;
    

    private bool isPanelOpened;

    void Awake()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void RecreatePanel()
    {
        if (panel != null)
            Destroy(panel);

        panel = Instantiate(panelPrefab, canvas.transform);
        inputSlotsTransform = panel.GetComponent<CraftingBlockPanelUI>().InputSlots.transform;
        outputSlotsTransform = panel.GetComponent<CraftingBlockPanelUI>().OutputSlots.transform;
        RecreateInputSlots();
        RecreateOutputSlots();
    }


    public void RecreateInputSlots()
    {
        foreach (GameObject slot in inputSlotsTransform)
            Destroy(slot);

        Slot[] inputSlots = gameObject.GetComponent<CraftingBlock>().inputSlots;
        for (int i = 0; i < inputSlots.Length; i++)
        {
            int x = inputSlotsXOffset + (i * (distanceBetweenSlots + slotSize));
            int y = inputSlotsYOffset;
            RedactInputSlot(Instantiate(inputSlotPrefab, new Vector3(x, y), new Quaternion(0, 0, 0, 0), inputSlotsTransform), i);
        }
        ReloadInputSlots();
    }

    public void RecreateOutputSlots()
    {
        foreach (GameObject slot in inputSlotsTransform)
            Destroy(slot);

        Slot[] inputSlots = gameObject.GetComponent<CraftingBlock>().inputSlots;
        for (int i = 0; i < inputSlots.Length; i++)
        {
            int x = outputSlotsXOffset + (i * (distanceBetweenSlots + slotSize));
            int y = outputSlotsYOffset;

            RedactOutputSlot(Instantiate(outputSlotPrefab, new Vector3(x, y), new Quaternion(0, 0, 0, 0), outputSlotsTransform), i);
        }
        ReloadOutputSlots();
    }
    

    protected abstract void RedactInputSlot(GameObject slotObject, int slotIndex);//????????? slotIndex ? ???? ???? ??
    protected abstract void RedactOutputSlot(GameObject slotObject, int slotIndex);//????????? slotIndex ? ???? ???? ??

    public void ReloadInputSlots()
    {
        foreach (GameObject slot in inputSlotsTransform)
            slot.GetComponent<CraftingBlockInputSlotUI>().ReloadUI();
    }

    public void ReloadOutputSlots()
    {
        foreach (GameObject slot in outputSlotsTransform)
            slot.GetComponent<CraftingBlockOutputSlotUI>().ReloadUI();
    }

    public void OpenOrClosePanel()
    {
        isPanelOpened = !isPanelOpened;
        if (!isPanelOpened)
            Destroy(panel);
        else
            RecreatePanel();
    }

    public void Interact(GameObject inventoryObject)
    {
        OpenOrClosePanel();
    }
}
