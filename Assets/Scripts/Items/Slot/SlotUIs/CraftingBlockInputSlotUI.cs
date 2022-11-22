using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBlockInputSlotUI : SlotUI
{
    GameObject craftingBlock;
    int slotIndex;
    public override Slot GetSlot() => craftingBlock.GetComponent<CraftingBlock>().inputSlots[slotIndex];

    public override void SetSlot(Slot slot)
    {
        craftingBlock.GetComponent<CraftingBlock>().inputSlots[slotIndex] = slot;
    }
}
