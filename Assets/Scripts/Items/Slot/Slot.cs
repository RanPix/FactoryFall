using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class Slot
{
    [Header("Main")]

    [SerializeField] private int slotIndex;
    [SerializeField] public Item item;
    [SerializeField] private GameObject inventoryObject;

    [Header("Filtering")]

    [SerializeField] private bool isFiltering;
    [SerializeField] public ItemType filteringType;
}