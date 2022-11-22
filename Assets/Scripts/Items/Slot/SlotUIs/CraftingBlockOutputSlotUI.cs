using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBlockOutputSlotUI : SlotUI
{
    GameObject craftingBlock;
    int slotIndex;
    public override Slot GetSlot() => craftingBlock.GetComponent<CraftingBlock>().inputSlots[slotIndex];

    public override void SetSlot(Slot slot)
    {
        craftingBlock.GetComponent<CraftingBlock>().outputSlots[slotIndex] = slot;
    }
}
