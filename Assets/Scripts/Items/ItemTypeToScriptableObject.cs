using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class ItemTypeToScriptableObject : MonoBehaviour
    {
        [SerializeField] private List<ItemTypeInfo> itemTypeInfos;

        public ItemTypeInfo GetItemTypeInfo(ItemType itemType)
        {
            foreach (ItemTypeInfo itemTypeInfo in itemTypeInfos)
            {
                if (itemTypeInfo.itemType == itemType)
                {
                    return itemTypeInfo;
                }
            }
            return itemTypeInfos[0];
        }
    }
}