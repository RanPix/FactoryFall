using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Nothing,
    IronOre,
    CopperOre,
}

public class Item
{
    public static Dictionary<ItemType, int> itemTypesMaxStacks = new Dictionary<ItemType, int>()
    {
        { ItemType.Nothing, 99999 },
        { ItemType.IronOre, 100 },
        { ItemType.CopperOre, 100 },
    };

    public int count = 0;
    public ItemType itemType = ItemType.Nothing;

    public Item(int count = 0, ItemType itemType = ItemType.Nothing)
    {
        this.count = count;
        this.itemType = itemType;
    }
    
    public Item PutItem(Item item)//пояснення чому не void: я хочу щоб на мишці висів 1 item типу як предмет який тримає мишка, і при визові цієї функції вміст мишки буде замінюватись на Item який повертає ця функція
    {
        if (itemType != item.itemType)
        {
            Item TakenItem = new Item(count, itemType);
            count = item.count;
            itemType = item.itemType;

            return TakenItem;
        }
        else
        {
            if (item.count + count <= itemTypesMaxStacks[item.itemType])
            {
                count += item.count;
                return new Item();
            }
            else
            {
                item.count -= itemTypesMaxStacks[item.itemType] - count;
                count = itemTypesMaxStacks[item.itemType];
                return item;
            }
        }
        //return item;
    }

    public Item SecondaryPutItem(Item item)//пояснення чому не void: я хочу щоб на мишці висів 1 item типу як предмет який тримає мишка, і при визові цієї функції вміст мишки буде замінюватись на Item який повертає ця функція
    {
        if (item.itemType == ItemType.Nothing && itemType != ItemType.Nothing)
        {
            int itemsLeft = count / 2;
            int itemsTake = itemsLeft;
            if (count % 2 != 0)
            {
                itemsTake++;
            }
            count = itemsLeft;
            return new Item(itemsTake, itemType);
        }

        else if ((item.itemType == itemType && count < itemTypesMaxStacks[itemType]) || item.itemType == ItemType.Nothing)
        {
            itemType = item.itemType;
            item.count--;
            count++;
        }

        return item;
    }
    
    public override string ToString() => $"{count} {itemType}";

    public static Item operator +(Item firstItem, Item secondItem) => new Item(firstItem.count + secondItem.count, firstItem.itemType);

    public static bool operator >(Item firstItem, Item secondItem) => firstItem.count > secondItem.count;

    public static bool operator <(Item firstItem, Item secondItem) => firstItem.count < secondItem.count;

    public static bool operator >(int count, Item item) => count > item.count;

    public static bool operator <(int count, Item item) => count < item.count;

    public static bool operator >(Item item, int count) => count > item.count;

    public static bool operator <(Item item, int count) => item.count < count;

    public static bool operator >(Item[] itemArray, Item item)
    {
        int a = 0;
        foreach (Item _item in itemArray)
        {
            if (_item.itemType == item.itemType)
            {
                a += _item.count;
                if (a > _item.count) return true;
            }
        }
        return false;
    }

    public static bool operator <(Item[] itemArray, Item item)
    {
        int a = 0;
        foreach (Item _item in itemArray)
        {
            if (_item.itemType == item.itemType)
            {
                a += _item.count;
                if (!(a < item.count)) return false;
            }
        }
        return true;
    }

    public static bool operator >(List<Item> itemArray, Item item)
    {
        int a = 0;
        foreach (Item _item in itemArray)
        {
            if (_item.itemType == item.itemType)
            {
                a += _item.count;
                if (a > _item.count) return true;
            }
        }
        return false;
    }

    public static bool operator <(List<Item> itemArray, Item item)
    {
        int a = 0;
        foreach (Item _item in itemArray)
        {
            if (_item.itemType == item.itemType)
            {
                a += _item.count;
                if (!(a < item.count)) return false;
            }
        }
        return true;
    }
}