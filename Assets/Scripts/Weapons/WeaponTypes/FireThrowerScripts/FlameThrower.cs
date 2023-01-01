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
        Debug.Log("piu");
        _nextFire = Time.time;
        //RayCasting();
        if (!fireTriggerEnable)
        {
            InstantiateFireTrigger();
        }
        if(useAudio)
            PlayShootSound();
        SpawmMuzzle();

        weaponAmmo.Ammo--;
        weaponAmmo.ApdateAmmoInScreen();
        if (useAnimations == true)
            animator.Play(shootAnimationName);
        return new Ray();
    }
    public void InstantiateFireTrigger()
    {
        fireTriggerEnable = true;
        FlameThrowerTrigger.enabled = true;
    }
    public override void Scope()
    {

    }
    private new void SpawmMuzzle()
    {
        if (attachment.hasSilencer == false)
        {
            if (weaponScriptableObject.haveMuzzle == true)
            {
                GameObject currentMuzzle = muzzle[Random.Range(0, muzzle.Length)];
                GameObject spawnedMuzzle = Instantiate(currentMuzzle, muzzlePosition.transform.position, muzzlePosition.rotation, gameObject.transform);
                spawnedMuzzle.transform.localScale = new Vector3(weaponScriptableObject.scaleFactor, weaponScriptableObject.scaleFactor, weaponScriptableObject.scaleFactor);
                Destroy(spawnedMuzzle, weaponScriptableObject.TimeTodestroy);
            }
        }
    }
    public override void FireButtonWasReleased()
    {
        fireTriggerEnable = false;
        FlameThrowerTrigger.enabled = false;
    }

}
