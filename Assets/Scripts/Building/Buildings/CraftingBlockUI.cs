using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBlockUI : MonoBehaviour
{
    [Header("Prefabs")]

    [SerializeField] GameObject panelPrefab;
    [SerializeField] GameObject inputSlotPrefab;
    [SerializeField] GameObject outputSlotPrefab;

    [Header("Slots Positioning Info")]

    [SerializeField] int slotSize;
    [SerializeField] int distanceBetweenSlots;
    [SerializeField] int inputSlotsXOffset;
    [SerializeField] int inputSlotsYOffset;
    [SerializeField] int outputSlotsXOffset;
    [SerializeField] int outputSlotsYOffset;

    public GameObject canvas;
    private GameObject panel;
    private GameObject[] inputSlots;
    private GameObject[] outputSlots;

    public void ReloadPanel()
    {
        if (panel != null)
            Destroy(panel);

        panel = Instantiate(panelPrefab, canvas.transform);
    }

    public void ReloadInputSlots()
    {
        CraftingBlock craftingBlock = gameObject.GetComponent<CraftingBlock>();
        if (craftingBlock == null)
            return;
    }

    public void ReloadOutputSlots()
    {

    }
}
