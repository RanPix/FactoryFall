using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class AttackZoneTriggerScript : MonoBehaviour
{
    private Metalon MetalonParent;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GamePlayer>())
        {
            MetalonParent.enemiesInAttackZone.Add(other.gameObject);
            if (MetalonParent.enemiesInTargetZone.Contains(other.gameObject))
            {
                MetalonParent.enemiesInTargetZone.Remove(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GamePlayer>())
            MetalonParent.enemiesInAttackZone.Remove(other.gameObject);
    }

}
