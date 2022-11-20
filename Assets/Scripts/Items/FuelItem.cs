using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class FuelItem : Item
    {
        public int FuelValue;

        public FuelItem(int count = 0, ItemType itemType = ItemType.Nothing)
        {
            this.count = count;
            this.itemType = itemType;
            this.FuelValue = ((FuelItemTypeInfo)MonoBehaviour.FindObjectOfType<ItemTypeToScriptableObject>().GetItemTypeInfo(itemType)).fuelValue;
        }
    }
}