using UnityEngine;

public static class PhysicsHelper
{
    public static Collider GetClosestToGO(Vector3 objPos, Vector3 checkPos, float radius, int layerMask)
    {
        Collider[] colls = Physics.OverlapSphere(checkPos, radius, layerMask, QueryTriggerInteraction.Ignore);

        if (colls.Length <= 0) return null;

        float computedLen = (colls[0].transform.position - objPos).magnitude;
        float closestPos = computedLen;
        int closestIndex = 0;

        for (int i = 0; i < colls.Length; i++)
        {
            computedLen = (colls[i].transform.position - objPos).magnitude;

            if (computedLen < closestPos)
            {
                closestPos = computedLen;
                closestIndex = i;
            }
        }

        return colls[closestIndex];
    }

    // finish it sdflgsajlg
    /*public static Collider[] GetClosestToGO(Vector3 objPos, Vector3 checkPos, float radius, int layerMask, int amount)
    {
        Collider[] colls = Physics.OverlapSphere(checkPos, radius, layerMask, QueryTriggerInteraction.Ignore);

        float computedLen = (colls[0].transform.position - objPos).magnitude;
        float closestPos = computedLen;
        int closestIndex = 0;

        for (int i = 0; i < colls.Length; i++)
        {
            computedLen = (colls[i].transform.position - objPos).magnitude;

            if (computedLen < closestPos)
            {
                closestPos = computedLen;
                closestIndex = i;
            }
        }

        return colls;
    }*/
}
