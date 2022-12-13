using UnityEngine;
using Player;

public class TargetZoneTriggerScript : MonoBehaviour
{
    private Metalon MetalonParent;
    // Start is called before the first frame update
    void Start()
    {
        MetalonParent = transform.GetComponentInParent<Metalon>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GamePlayer>())
        {
            MetalonParent.enemiesInTargetZone.Add(other.gameObject);
            if (MetalonParent.enemiesInAttackZone.Contains(other.gameObject))
            {
                MetalonParent.enemiesInAttackZone.Remove(other.gameObject);
            }
            
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GamePlayer>())
            MetalonParent.enemiesInTargetZone.Remove(other.gameObject);
    }
}
