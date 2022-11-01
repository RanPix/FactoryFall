using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    public enum states { Active, Destroied }
    public enum type { Food, Weapon, Instrument };

    private PlayerControls controls;

    [SerializeField] private _GunType gunType;
    [SerializeField] private _ShootType shootType;
    [SerializeField] private states _state;
    [Space(10)]
    [SerializeField] private float damage;
    [SerializeField] private float headDamage;
    [SerializeField] private float fireRate;
    [SerializeField] private float weaponRange;
    [SerializeField] private float reloadTime;
    [SerializeField] private LayerMask playerMask;


    [Header("Animation")]
    [SerializeField] private bool useAnimations;
    [Space(5)]
    [SerializeField] private string shootAnimationName;
    [SerializeField] private string reloadAnimationTriggername;

    [Header("Sound")]
    [SerializeField] private AudioClip empty;
    [SerializeField] private AudioClip reload;

    [Space(10)]
    [SerializeField] private AudioClip[] shoots;

    [Space(10)]
    [SerializeField] private AudioClip[] shootsSilencer;

    [Header("Sway")]
    [SerializeField] private bool canSway;
    [SerializeField] private Vector3 initialSwayPosition;
    [SerializeField] private float SwayAmount;
    [SerializeField] private float MaxSwayAmount;
    [SerializeField] private float swaySmoothing;

    [Space(10)]
    [SerializeField] private Attachment attachment;

    [Header("Scope")]
    [SerializeField] private float normalFOV;
    [SerializeField] private float scopedFOV;
    [SerializeField] private float normalSens;
    [SerializeField] private float scopedSens;
    [SerializeField] private float FOVSmoothing;

    [Header("Aiming")]
    [SerializeField] private bool canScope;
    [SerializeField] private Vector3 normalLOcalPosition;
    [SerializeField] private Vector3 aimingLOcalPosition;
    [SerializeField] private float aimSmoothing;


    [Header("Ammo")]
    [SerializeField] private string ammoObjectName;
    public int ammo;
    public int maxAmmo;
    public int reserveAmmo;

    [Header("Muzzle")]
    [SerializeField] private bool haveMuzzle;
    [SerializeField] private Transform MuzzlePosition;
    [SerializeField] private float sczaleFactor;
    [SerializeField] private float TimeTodestroy;

    [Space(10)]
    [SerializeField] private GameObject[] muzzle;

    [Header("Physics Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletSpawner;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletTimeToDestroy;

    [Header("Throw Away")]
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private GameObject weaponItem;

    [SerializeField] private GameObject M4A1;

    private enum _GunType
    {
        Auto,
        Semi
    }
    private enum _ShootType
    {
        ray,
        physics,
    }
    [SerializeField] public bool canShoot;
    private float _nextFire;
    private Camera _cam;
    private Camera _gunCam;
    private AudioSource _audioSource;
    private AudioSource _audioShootSource;
    private Animator _animator;
    private TMP_Text _ammoText;
    public WeaponAmmo _weaponAmmo;
    public bool inScope 
    { 
        get => _inScope;
        private set 
        {
            _inScope = value;
            OnInScopeValuseChange?.Invoke();

        }
    }
    private bool _inScope;
    public ConnectorHelper connectorHelper;
    public Action OnInScopeValuseChange;
    private void Awake()
    {
        _state = states.Active;
        controls = new PlayerControls();
        controls.Enable();
        _cam = Camera.main;
        _audioShootSource = gameObject.GetComponent<AudioSource>();
        _animator = gameObject.GetComponent<Animator>();
    }
    private void OnDestroy()
    {
        _state = states.Destroied;
    }
    public void weaponAmmoUpdate()
    {
        if (_weaponAmmo)
        {
            _weaponAmmo.Ammo = ammo;
            _weaponAmmo.ClipSize = maxAmmo;
            _weaponAmmo.ReserveAmmo = reserveAmmo;
        }

    }


    private void Start()
    {
        connectorHelper = ConnectorHelper.Instance;
        weaponHolder = connectorHelper.weaponHolder;
        _gunCam = connectorHelper.gunCam;
        _audioSource = connectorHelper.player.GetComponent<AudioSource>();
        _ammoText = connectorHelper.ammoText;
        _weaponAmmo = connectorHelper.weaponAmmo;

        canShoot = true;
        weaponAmmoUpdate();
        _weaponAmmo.AmmoText = _ammoText;
        _weaponAmmo.ClipSize = maxAmmo;
        OnInScopeValuseChange += Scope;

    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case states.Active:
                    
                    Aiming();
                    KeyCodes();
                break;
        }

    }
    private void KeyCodes()
    {
        if (canShoot == true)
        {
            if (_weaponAmmo.Ammo > 0)
            {
                if (Time.time - _nextFire > 1 / fireRate)
                {
                    if (gunType == _GunType.Auto && controls.Player.Fire.IsPressed())
                    {
                        Shoot();
                    }
                    else if (gunType == _GunType.Semi && controls.Player.Fire.WasPerformedThisFrame())
                    {
                        Shoot();
                    }
                }
            }
        }
        if (controls.Player.Fire.IsPressed() && _weaponAmmo.Ammo <= 0)
        {
            _audioShootSource.PlayOneShot(empty);
        }
        if (controls.Player.Reload.WasPerformedThisFrame())
        {
            if (_weaponAmmo.ReserveAmmo > 0 && _weaponAmmo.Ammo < ammo)
            {
                StartCoroutine(ReloadCoroutine());
            }
        }
    }



    private void Scope()
    {
        if (_inScope == true)
        {
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, scopedFOV, FOVSmoothing * Time.deltaTime);
            _gunCam.fieldOfView = Mathf.Lerp(_gunCam.fieldOfView, scopedFOV, FOVSmoothing * Time.deltaTime);
        }
        else if (_inScope == false)
        {
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, normalFOV, FOVSmoothing * Time.deltaTime);
            _gunCam.fieldOfView = Mathf.Lerp(_gunCam.fieldOfView, normalFOV, FOVSmoothing * Time.deltaTime);
        }
    }







    private void Aiming()
    {
        Vector3 target = normalLOcalPosition;

        if (controls.Player.Scope.IsPressed())
        {
            initialSwayPosition = attachment.positionsInScope[attachment.Scopeid];
            target = attachment.positionsInScope[attachment.Scopeid];
            _inScope = true;
        }
        else if (_inScope == true)
        {
            initialSwayPosition = normalLOcalPosition;
            _inScope = false;

        }

        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);
        transform.localPosition = desiredPosition;
    }




    private void Shoot()
    {
        _nextFire = Time.time;
        if (shootType == _ShootType.ray)
        {
            RayCasting();
        }
        else if (shootType == _ShootType.physics)
        {
            SpawnBullet();
        }
        PlayShootSound();
        SpawmMuzzle();

        _weaponAmmo.Ammo--;
        _weaponAmmo.ApdataAmmoInScreen();
        if(useAnimations == true)
            _animator.Play(shootAnimationName);
    }

    private void SpawnBullet()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab);
        //spawnedBullet.transform.SetParent(weaponBody);
        spawnedBullet.transform.position = bulletSpawner.transform.position;
        spawnedBullet.GetComponent<Bullet>().AddForceBullet(bulletSpawner.transform.forward * bulletSpeed);
        Destroy(spawnedBullet, bulletTimeToDestroy);
    }

    private void RayCasting()
    {

        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, weaponRange, playerMask))
        {
            Particle particleGO = hit.transform.GetComponent<Particle>();
            if (particleGO)
            {
                GameObject particle = Instantiate(particleGO.particle.particleObject, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(particle, TimeTodestroy);
            }
            




            // так будемо додавати ворогів по яким буде проходити урон  
            /*GroundEnemy ork = hit.transform.GetComponent<GroundEnemy>();
            if (ork)
            {
                ork.HP -= damage;
            }*/
        }
    }


    private void PlayShootSound()
    {
        AudioClip clip = null;
        if (attachment.haveSilencer == true)
        {
            clip = shootsSilencer[Random.Range(0, shoots.Length)];
            _audioShootSource.clip = clip;
            _audioShootSource.Play();

        }
        else if (attachment.haveSilencer == false)
        {
            clip = shoots[Random.Range(0, shoots.Length)];
            _audioShootSource.clip = clip;
            _audioShootSource.Play();

        }
    }

    private void SpawmMuzzle()
    {
        if (attachment.haveSilencer == false)
        {
            if (haveMuzzle == true)
            {
                GameObject currentMuzzle = muzzle[Random.Range(0, muzzle.Length)];
                GameObject spawnedMuzzle = Instantiate(currentMuzzle, MuzzlePosition.position, MuzzlePosition.rotation);
                spawnedMuzzle.transform.localScale = new Vector3(sczaleFactor, sczaleFactor, sczaleFactor);
                Destroy(spawnedMuzzle, TimeTodestroy);
            }
        }
    }
    private IEnumerator ReloadCoroutine()
    {
        canShoot = false;
        if(useAnimations == true)
            _animator.SetTrigger(reloadAnimationTriggername);
        _audioShootSource.PlayOneShot(reload);

        yield return new WaitForSeconds(reloadTime);

        _weaponAmmo.AddAmmo();
        _weaponAmmo.ApdataAmmoInScreen();
        canShoot = true;
    }


}

[System.Serializable]
public class Attachment
{
    [Header("Silencer")]
    public bool haveSilencer;
    public GameObject silencerObject;

    [Header("Scope`s")]
    public int Scopeid;
    public GameObject ironSight;

    [Space(10)]
    public AttachmentInfo[] scopes;
    public Vector3[] positionsInScope;

}


