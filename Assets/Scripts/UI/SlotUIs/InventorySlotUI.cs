using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotUI : SlotUI
{
    public int slotIndex;
    public override Slot GetSlot() => inventoryObject != null ? inventoryObject.GetComponent<Inventory>().slots[slotIndex] : new Slot();

    public override void SetSlot(Slot slot)
    {
        inventoryObject.GetComponent<Inventory>().slots[slotIndex] = slot;
    }
}
