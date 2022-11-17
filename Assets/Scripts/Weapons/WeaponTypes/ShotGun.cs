using UnityEngine;

using UnityEngine.UI;
public class ShotGun : Weapon
{
    #region AbstractVariables
    protected override float nextFire => _nextFire;
    #endregion
    private float _nextFire = 0;


    public override void Shoot()
    {
        Debug.Log("piu");
        _nextFire = Time.time;
        for (int i = 0; i < 10; i++)
        {
            RayCasting();
            
        }
        if (useAudio)
            PlayShootSound();
        SpawmMuzzle();

        weaponAmmo.Ammo--;
        weaponAmmo.ApdateAmmoInScreen();
        if (useAnimations == true)
            animator.Play(shootAnimationName);
    }
    public override void Scope()
    {

    }
    protected override void FireButtonWasReleased()
    {
    }
    protected new void RayCasting()
    {
        RaycastHit hit;
        Vector3 V3 = new Vector3(cam.transform.forward.x + Random.Range(-0.1f, 0.1f), cam.transform.forward.y + Random.Range(-0.1f, 0.1f), cam.transform.forward.z);
        if (Physics.Raycast(cam.transform.position, V3, out hit, weaponScriptableObject.weaponShootRange, playerMask))
        {

        }
        Debug.DrawRay(cam.transform.position, V3*10, Color.green, 2);
    }
}
