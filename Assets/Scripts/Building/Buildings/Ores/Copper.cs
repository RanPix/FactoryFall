using UnityEngine;
using Mirror;

public class Copper : Ore
{
    private void Start()
    {
        isDestroyable = false;
    }

    public override bool Damage(float damage)
    {
        if (defence > damage)
            return false;

        dmgDone += damage;

        if (dmgDone < dmgToDropItem)
            return false;

        dmgDone = 0;
        DropItem();

        return true;
    }

    private void DropItem()
    {
        print("digdig copper");
    }
}
