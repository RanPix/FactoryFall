using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Weapon
{
    #region AbstractVariables
    public override float nextFire => _nextFire;
    #endregion
    private float _nextFire = 0;
    private bool fireTriggerEnable = false;
    [SerializeField] private FlameThrowerTrigger flameThrowerTriggerGO;
    private FlameThrowerTrigger FlameThrowerTrigger;


    public override Ray Shoot()
    {
        //Debug.Log("piu");
        _nextFire = Time.time;

        if (!fireTriggerEnable)
        {
            InstantiateFireTrigger();
        }
        SpawnMuzzle();

        weaponAmmo.Ammo--;
        weaponAmmo.ApdateAmmoInScreen();

        animator.Play(shootAnimationName);

        return new Ray();
    }

    public void InstantiateFireTrigger()
    {
        fireTriggerEnable = true;
        FlameThrowerTrigger.enabled = true;
    }

    public override void FireButtonWasReleased()
    {
        fireTriggerEnable = false;
        FlameThrowerTrigger.enabled = false;
    }

}
