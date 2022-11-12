using System.Collections;
using UnityEngine;

public class Turret : Block
{
    [SerializeField] private LayerMask targetTeamLM;
    [SerializeField] private float searchCallTime;

    [SerializeField] protected Transform target; // TEST SERIALIZATION

    [Header("Stats")]

    [SerializeField] protected float shootingDistance;
    [SerializeField] protected float damage;
    [SerializeField] protected float shootTime;
    protected float shootTimer;

    protected RaycastHit hit;

    [Space]

    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int curAmmo;

    [SerializeField] protected int consumeAmmoPerShot;

    [Space]

    [SerializeField] protected Transform turretHead;
    [SerializeField] protected Transform[] muzzlePositions;
    protected int muzzleOrder;

    void Start()
    {
        StartCoroutine(Search());
    }

    private IEnumerator Search()
    {
        Collider detected = null;

        while (true)
        {
            if (target != null)
                yield return new WaitForSeconds(searchCallTime + searchCallTime);
            
            detected = PhysicsHelper.GetClosestToGO(transform.position, transform.position, shootingDistance, targetTeamLM);

            if (detected != null)
                target = detected.transform;

            yield return new WaitForSeconds(searchCallTime);
        }
    }

    protected virtual void Shoot() { }
}
