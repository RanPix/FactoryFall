using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class Inventory : MonoBehaviour
{
    public int slotsCount = 36;
    public Slot[] slots;

    void Awake()
    {
        slots = new Slot[slotsCount];
        for(int i = 0; i < slotsCount; i++)
            slots[i] = new Slot();
        slots[3] = new Slot(23, ItemType.CopperOre);
        slots[19] = new Slot(89, ItemType.CopperOre);
        slots[16] = new Slot(82, ItemType.IronOre);
        slots[15] = new Slot(83, ItemType.IronOre);
    }

    public void Sort()
    {
        //дороблюй сергійко, дороблюй
        //колись зроблю, Бодя казав можна потім
        //чи буду я це взагалі робити ближчим часом?
        //мабуть ні
        //точно не в прототипі... хочааа
        //блін, а коли я це зроблю? вже перший день праці на альфі, ми відпочивали 3 дні
        //мабуть не зроблю...
    }
}
