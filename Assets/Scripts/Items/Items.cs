using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemsTypes
{
    Nothing,
    IronOre,
    CopperOre,
    Coal,
}


public class Item
{
    static Dictionary<ItemsTypes, int> itemTypesMaxStacks = new Dictionary<ItemsTypes, int>()
    {
        { ItemsTypes.Nothing, 0 },
        { ItemsTypes.IronOre, 100 },
        { ItemsTypes.CopperOre, 100 },
    };

    public int count = 0;
    public new ItemsTypes itemType = ItemsTypes.Nothing;

    public Item(int count = 0, ItemsTypes itemType = ItemsTypes.Nothing)
    {
        this.count = count;
        this.itemType = itemType;
    }
    
    public Item putItem(Item item)//пояснення чому не void: я хочу щоб на мишці висів 1 item типу як предмет який тримає мишка, і при визові цієї функції вміст мишки буде замінюватись на Item який повертає ця функція
    {
        if (itemType != item.itemType)
        {
            Item TakenItem = this;

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
        return new Item();
    }

    public static Item operator +(Item firstItem, Item secondItem) => new Item(firstItem.count + secondItem.count, firstItem.itemType);

    public static bool operator >(Item firstItem, Item secondItem) => firstItem.count > secondItem.count;

    public static bool operator <(Item firstItem, Item secondItem) => firstItem.count < secondItem.count;

    public static bool operator >(Item[] itemArray, Item item)
    {
        foreach (Item _item in itemArray)
        {
            if (_item.itemType == item.itemType)
            {
                return item.count > _item.count;
            }
        }
        return false;
    }

    public static bool operator <(Item[] itemArray, Item item)
    {
        foreach (Item _item in itemArray)
        {
            if (_item.itemType == item.itemType)
            {
                return item.count < _item.count;
            }
        }
        return false;
    }

    public static bool operator >(List<Item> itemArray, Item item)
    {
        foreach (Item _item in itemArray)
        {
            if (_item.itemType == item.itemType)
            {
                return item.count > _item.count;
            }
        }
        return false;
    }

    public static bool operator <(List<Item> itemArray, Item item)
    {
        foreach (Item _item in itemArray)
        {
            if (_item.itemType == item.itemType)
            {
                return item.count < _item.count;
            }
        }
        return false;
    }
}