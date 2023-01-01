using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Pistol : Weapon
{
    #region AbstractVariables
    public override float nextFire => _nextFire;
    #endregion
    private float _nextFire = 0;


    public override Ray Shoot()
    {
        Debug.Log("piu");
        _nextFire = Time.time;
        if(useAudio)
            PlayShootSound();
        SpawmMuzzle();

        weaponAmmo.Ammo--;
        weaponAmmo.ApdateAmmoInScreen();
        if (useAnimations == true)
            animator.Play(shootAnimationName);
        return GetRay();
    }
    public override void Scope()
    {

    }
    public override void FireButtonWasReleased()
    {
    }
}
