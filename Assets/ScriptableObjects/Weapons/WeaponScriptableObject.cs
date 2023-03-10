using UnityEngine;

[CreateAssetMenu(fileName ="WeaponInfo", menuName = "Weapon/New Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [Space(10)] 

    public string type;
    public string name;
    public int damage;


    [Space(10)] 

    public float shootRange;
    public bool useOneAmmoPerShot;
    public int maxAmmo;
    public int numberOfBulletsPerShot;
    public float timeBetweenSpawnBullets;
    public Vector2[] patern;

    public float timeBetweenShots;

    public float reloadTime;

    [Space]
    [Header("Audio")]
    public AudioClip empty;
    public AudioClip reload;

    [Space(5)]
    public AudioClip[] shotSounds;

    [Space]
    [Header("Muzzle")]
    public float muzzleFlashLifetime;
    public Transform muzzleFlash;

    [Space]
    [Header("Physics Shoot")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float bulletTimeToDestroy;
}
