using UnityEngine;

public class Gun : Weapon
{
    #region AbstractVariables
    public override float nextFire => _nextFire;
    #endregion
    private float _nextFire = 0;

    
    public override Ray Shoot()
    {
        //Debug.Log("piu");
        _nextFire = Time.time;
        SpawnMuzzle();

        weaponAmmo.Ammo--;
        weaponAmmo.UpdateAmmoInScreen();

        animator.Play(shootAnimationName);

        //SpawnTrail();

        return GetRay();
    }


    public override void FireButtonWasReleased()
    {
    }
}
