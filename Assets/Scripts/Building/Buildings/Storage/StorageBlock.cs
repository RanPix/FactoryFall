using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBlock : Block
{
    public Slot[] slots;

    [Header("Storage Block Parameters")]

    [SerializeField] int slotsCount;

    void Start()
    {
        slots = new Slot[slotsCount];

        for (int i = 0; i < slotsCount; i++)
        {
            slots[i] = new Slot();
        }
    }
}
