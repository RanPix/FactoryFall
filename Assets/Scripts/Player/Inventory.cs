/*s*/using System.Collections;
/*s*/using System.Collections.Generic;
/*s*/using UnityEngine;
/*s*/using Items;

public class Inventory : MonoBehaviour
{
    private int slotsCount;//зробіть тут get set, я не знаю що тут краще ставити
    private Item[] items;//зробіть тут get set, я не знаю що тут краще ставити

    void Awake()
    {
        slotsCount = 100;
        items = new Item[slotsCount];
        for(int i = 0; i < slotsCount; i++)
        {
            items[i] = new Item();
        }
    }

    public Item PutItem(int slotIndex, Item item)//пояснення чому не void: я хочу щоб на мишці висів 1 item типу як предмет який тримає мишка, і при визові цієї функції вміст мишки буде замінюватись на Item який повертає ця функція
    {
        if (item.itemType != items[slotIndex].itemType)
        {
            Item TakenItem = items[slotIndex];
            items[slotIndex] = item;
            return TakenItem;
        }
        else
        {
            if (item.count + items[slotIndex].count <= (int)item.itemType)
            {
                items[slotIndex].count += item.count;
                return new Item();
            }
            else
            {
                item.count -= (int)item.itemType - items[slotIndex].count;
                items[slotIndex].count = (int)item.itemType;
                return item;
            }
        }
        return new Item();
    }

    public void Sort()
    {
        //дороблюй сергійко, дороблюй
    }

    void Update()
    {
        Debug.Log(new ItemIronOre(1).GetType());
    }
}
