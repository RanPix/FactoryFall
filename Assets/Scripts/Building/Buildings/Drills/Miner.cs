using UnityEngine;
using Mirror;

public class Miner : Drill
{
    private void Update()
    {
        if (!isServer)
            return;

        Mine();
    }

    private void Mine()
    {
        digTimer += Time.deltaTime;

        if (digTimer < diggingTime)
            return;

        digTimer = 0f;

        Collider[] ores = Physics.OverlapSphere(diggingPos.position, diggingRadius, oreLM);
        if (ores == null)
            return;

        for (int i = 0; i < ores.Length; i++)
        {
            if (damageDone < ores[i].GetComponent<Ore>().GetDamageToDropItem())
                damageDone += diggingStrength;
            else
                inventory++;
        }
    }
}
