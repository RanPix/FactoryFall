using UnityEngine;

public class PhysicsShootType : Weapon
{
    public void Shoot()
    {
        _nextFire = Time.time;
        SpawnBullet();
        PlayShootSound();
        SpawmMuzzle();

        _weaponAmmo.Ammo--;
        _weaponAmmo.ApdataAmmoInScreen();
        if (useAnimations == true)
            _animator.Play(shootAnimationName);
    }

}
