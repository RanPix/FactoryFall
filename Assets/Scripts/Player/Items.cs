/*s*/using System.Collections;
/*s*/using System.Collections.Generic;
/*s*/using UnityEngine;

namespace Items
{
    public enum ItemTypes
    {
        Nothing,
        IronOre,
        CopperOre
    }

    public class Item
    {
        public int count;
        const int maxStack = 0;
        public ItemTypes itemType = ItemTypes.Nothing;
        public Item()
        {
            count = 0;
        }
    }

    public class ItemIronOre : Item
    {
        const int maxStack = 100;
        public ItemIronOre(int count)
        {
            ItemTypes itemType = ItemTypes.IronOre;
            this.count = count;
        }
    }
    public class ItemCopperOre : Item
    {
        const int maxStack = 100;
        public ItemCopperOre(int count)
        {
            ItemTypes itemType = ItemTypes.IronOre;
            this.count = count;
        }
    }
}