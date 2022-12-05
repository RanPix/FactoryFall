using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBlockSlotUI : SlotUI
{
    public GameObject storageBlock;
    int slotIndex;

    public override Slot GetSlot() 
        => storageBlock.GetComponent<StorageBlock>().slots[slotIndex];

    public override void SetSlot(Slot slot) 
        => storageBlock.GetComponent<StorageBlock>().slots[slotIndex] = slot;
}
