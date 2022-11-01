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
        slotsCount = 100;//полагодіть get set, я не знаю що тут краще ставити
        items = new Item[slotsCount];//полагодіть get set, я не знаю що тут краще ставити
        for(int i = 0; i < slotsCount; i++)
        {
            items[i] = new Item();
        }
    }

    public Item PutItem(int slotIndex, Item item)
    {
        if (item.itemType != items[slotIndex].itemType)
        {
            Item TakenItem = items[slotIndex];
            items[slotIndex] = item;
            return TakenItem;
        }
        else
        {
            //дороблюй сергійко, дороблюй
        }
        return new Item();
    }

    public void Sort()
    {
        //дороблюй сергійко, дороблюй
    }
}
