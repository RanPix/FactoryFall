/*s*/using System.Collections;
/*s*/using System.Collections.Generic;
/*s*/using UnityEngine;

namespace Items
{
    public enum ItemTypes
    {
        Nothing = 0,
        IronOre = 100,
        CopperOre = 100
    }

    public class Item
    {
        public int count;
        public ItemTypes itemType = ItemTypes.Nothing;
        public const int maxStack = (int)itemType;
        public Item()
        {
            count = 0;
        }
    }

    public class ItemIronOre : Item
    {
        public const ItemTypes itemType = ItemTypes.IronOre;
        public const int maxStack = (int)itemType;
        public ItemIronOre(int count)
        {
            this.count = count;
        }
    }

    public class ItemCopperOre : Item
    {
        public const ItemTypes itemType = ItemTypes.CopperOre;
        public const int maxStack = (int)itemType;
        public ItemCopperOre(int count)
        {
            this.count = count;
        }
    }
}