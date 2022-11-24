using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class FuelItem : Item
    {
        public int fuelValue;

        public FuelItem(int count = 0, ItemType itemType = ItemType.Nothing)
        {
            this.count = count;
            this.itemType = itemType;
            this.fuelValue = ((FuelItemTypeInfo)MonoBehaviour.FindObjectOfType<ItemTypeToScriptableObject>().GetItemTypeInfo(itemType)).FuelValue;
        }
    }
}