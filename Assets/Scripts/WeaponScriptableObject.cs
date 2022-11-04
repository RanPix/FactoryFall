using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="WeaponInfo", menuName = "Weapon/New Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public Weapon._GunType shootType;
    [Space(10)]
    public float damage;
    public float fireRate;
    public float weaponShootRange;
    public float reloadTime;

    [Space]
    [Header("Animation")]
    public string shootAnimationName;
    public string reloadAnimationTriggername;

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
    public bool haveMuzzle;
    public float scaleFactor;
    public float TimeTodestroy;

    [Space]
    [Header("Physics Shoot")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float bulletTimeToDestroy;








    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
