using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slotsCount = 36;//зробіть тут get set, я не знаю що тут краще ставити
    private Item[] items;//зробіть тут get set, я не знаю що тут краще ставити
    public Item cursorSlot = new Item();

    void Awake()
    {
        //slotsCount = 100;
        items = new Item[slotsCount];
        for(int i = 0; i < slotsCount; i++)
            items[i] = new Item();
    }

    public void Sort()
    {
        //дороблюй сергійко, дороблюй
        //колись зроблю, Бодя казав можна потім
        //чи буду я це взагалі робити ближчим часом?
    }
}
