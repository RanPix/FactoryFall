using UnityEngine;

public class Duo : Turret
{
    void Update()
    {
        if (!isServer)
            return;

        Shoot();
    }

    protected override void Shoot()
    {
        if (curAmmo < maxAmmo)
            return;

        if (target == null)
            return;

        //Quaternion headRotation = Quaternion.identity;
        //headRotation.SetFromToRotation(turretHead.position, target.position);

        turretHead.LookAt(target);


        shootTimer += Time.deltaTime;
        if (shootTimer < shootTime)
            return;
        shootTimer = 0;

        if (Physics.Raycast(muzzlePositions[muzzleOrder].position, turretHead.forward, out hit, shootingDistance))
            hit.transform.GetComponent<IDamagable>()?.Damage(damage);

        curAmmo -= consumeAmmoPerShot;

        if (muzzleOrder++ == muzzlePositions.Length - 1)
            muzzleOrder = 0;
    }
}
