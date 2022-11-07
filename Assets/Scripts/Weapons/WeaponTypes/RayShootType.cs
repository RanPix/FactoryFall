using UnityEngine;

public class RayShootType : Weapon
{

    public void Shoot()
    {
        _nextFire = Time.time;
        RayCasting();
        PlayShootSound();
        SpawmMuzzle();

        _weaponAmmo.Ammo--;
        _weaponAmmo.ApdataAmmoInScreen();
        if (useAnimations == true)
            _animator.Play(shootAnimationName);
    }
}
