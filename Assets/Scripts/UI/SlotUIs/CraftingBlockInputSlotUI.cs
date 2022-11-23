using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBlockInputSlotUI : SlotUI
{
    GameObject craftingBlock;
    int slotIndex;
    public override Slot GetSlot() => craftingBlock != null ? craftingBlock.GetComponent<CraftingBlock>().inputSlots[slotIndex] : new Slot();

    public override void SetSlot(Slot slot)
    {
        craftingBlock.GetComponent<CraftingBlock>().inputSlots[slotIndex] = slot;
    }
}
