using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName ="WeaponInfo", menuName = "Weapon/New Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [Space(10)]
    public int damage;
    public float fireRate;
    public float weaponShootRange;
    public float reloadTime;

    [Space]
    [Header("Audio")]
    public AudioClip empty;
    public AudioClip reload;

    [Space(5)]
    public AudioClip[] shoots;

    [Space(5)]
    public AudioClip[] shootsSilencer;    

    [Space]
    [Header("Muzzle")]
    public float muzzleFlashDeathTimer;
    public Transform muzzleFlash;

    [Space]
    [Header("Physics Shoot")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float bulletTimeToDestroy;
}
