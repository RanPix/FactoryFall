using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "Item Type Info", menuName = "ItemTypeInfo")]
    public class ItemTypeInfo : ScriptableObject
    {
        [SerializeField] private ItemType itemType;
        public ItemType ItemType => itemType;

        [SerializeField] private int maxStack;
        public int MaxStack => maxStack;

        [SerializeField] private string itemName;
        public string ItemName => itemName;
        [SerializeField] private string description;
        public string Description => description;

        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
    }
}