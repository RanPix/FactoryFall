using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class Inventory : MonoBehaviour
{
    public int slotsCount = 36;//зробіть тут get set, я не знаю що тут краще ставити
    public Item[] items;//зробіть тут get set, я не знаю що тут краще ставити
    public Item cursorSlot = new Item();

    void Awake()
    {
        items = new Item[slotsCount];
        for(int i = 0; i < slotsCount; i++)
            items[i] = new Item();
        items[3] = new Item(23, ItemType.CopperOre);
        items[19] = new Item(89, ItemType.CopperOre);
        items[16] = new Item(82, ItemType.IronOre);
        items[15] = new Item(83, ItemType.IronOre);
    }

    public void Sort()
    {
        //дороблюй сергійко, дороблюй
        //колись зроблю, Бодя казав можна потім
        //чи буду я це взагалі робити ближчим часом?
        //мабуть ні
        //точно не в прототипі... хочааа
    }
}
