using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEditor.IMGUI.Controls;
using Random = UnityEngine.Random;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    public enum states 
    { 
        Active,
        Destroied 
    }
    public enum ShootType 
    {
        Ray,
        Physics,
        Trigger
    }
    public enum _GunType
    {
        Auto,
        Semi
    }
    public WeaponScriptableObject weaponScriptableObject;
    [Header("Weapon`s shoot type scripts")]
    [SerializeField] private RayShootType rayShootType;
    [SerializeField] private PhysicsShootType physicsShootType;
    [SerializeField] private TriggetShootType triggerShootType;
    [Space(10)]

    [SerializeField] private _GunType gunType;
    [SerializeField] private ShootType shotType;
    [SerializeField] private states _state;
    [Space(10)]
    [SerializeField] private LayerMask playerMask;


    [Header("Animation")]
    public bool useAnimations;
    [Space(5)]
    public string shootAnimationName;
    [SerializeField] private string reloadAnimationTriggername;

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
    [SerializeField] private Transform MuzzlePosition;

    [Space(10)]
    [SerializeField] private GameObject[] muzzle;

    [Header("Physics Shoot")]
    [SerializeField] private GameObject bulletSpawner;
    [Space(15)]


    public bool canShoot;
    public Animator _animator;
    public float _nextFire;
    public WeaponAmmo _weaponAmmo;


    private PlayerControls controls;
    private Camera _cam;
    private Camera _gunCam;
    private AudioSource _audioSource;
    private TMP_Text _ammoText;

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
        _audioSource = gameObject.GetComponent<AudioSource>();
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
        _gunCam = connectorHelper.gunCam;
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
                if (Time.time - _nextFire > 1 / weaponScriptableObject.fireRate)
                {
                    if (shotType == ShootType.Trigger && gunType == _GunType.Auto && controls.Player.Fire.IsPressed())
                    {
                        switch (shotType)
                        {
                            case ShootType.Ray:
                                rayShootType.Shoot();
                                break;

                            case ShootType.Physics:
                                physicsShootType.Shoot();
                                break;
                        }
                    }
                    else if (shotType == ShootType.Trigger && gunType == _GunType.Semi && controls.Player.Fire.WasPerformedThisFrame())
                    {
                        switch (shotType)
                        {
                            case ShootType.Ray:
                                rayShootType.Shoot();
                                break;

                            case ShootType.Physics:
                                physicsShootType.Shoot();
                                break;
                        }
                    }
                    else if(shotType == ShootType.Trigger && controls.Player.Fire.WasPerformedThisFrame())
                    {

                    }
                    else if(shotType == ShootType.Trigger && controls.Player.Fire.WasReleasedThisFrame())
                    {

                    }
                }
            }
        }
        if (controls.Player.Fire.IsPressed() && _weaponAmmo.Ammo <= 0)
        {
            _audioSource.PlayOneShot(weaponScriptableObject.empty);
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




    public void SpawnBullet()
    {
        GameObject spawnedBullet = Instantiate(weaponScriptableObject.bulletPrefab);
        //spawnedBullet.transform.SetParent(weaponBody);
        spawnedBullet.transform.position = bulletSpawner.transform.position;
        spawnedBullet.GetComponent<Bullet>().AddForceBullet(bulletSpawner.transform.forward * weaponScriptableObject.bulletSpeed);
        Destroy(spawnedBullet, weaponScriptableObject.bulletTimeToDestroy);
    }

    public void RayCasting()
    {

        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, weaponScriptableObject.weaponShootRange, playerMask))
        {
            Particle particleGO = hit.transform.GetComponent<Particle>();
            if (particleGO)
            {
                GameObject particle = Instantiate(particleGO.particle.particleObject, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(particle, weaponScriptableObject.TimeTodestroy);
            }
            




            // так будемо додавати ворогів по яким буде проходити урон  
            /*GroundEnemy ork = hit.transform.GetComponent<GroundEnemy>();
            if (ork)
            {
                ork.HP -= damage;
            }*/
        }
    }


    public void PlayShootSound()
    {
        AudioClip clip = null;
        if (attachment.haveSilencer == true)
        {
            clip = weaponScriptableObject.shootsSilencer[Random.Range(0, weaponScriptableObject.shoots.Length)];
            _audioSource.clip = clip;
            _audioSource.Play();

        }
        else if (attachment.haveSilencer == false)
        {
            clip = weaponScriptableObject.shoots[Random.Range(0, weaponScriptableObject.shoots.Length)];
            _audioSource.clip = clip;
            _audioSource.Play();

        }
    }

    public void SpawmMuzzle()
    {
        if (attachment.haveSilencer == false)
        {
            if (weaponScriptableObject.haveMuzzle == true)
            {
                GameObject currentMuzzle = muzzle[Random.Range(0, muzzle.Length)];
                GameObject spawnedMuzzle = Instantiate(currentMuzzle, MuzzlePosition.position, MuzzlePosition.rotation);
                spawnedMuzzle.transform.localScale = new Vector3(weaponScriptableObject.scaleFactor, weaponScriptableObject.scaleFactor, weaponScriptableObject.scaleFactor);
                Destroy(spawnedMuzzle, weaponScriptableObject.TimeTodestroy);
            }
        }
    }
    private IEnumerator ReloadCoroutine()
    {
        canShoot = false;
        if(useAnimations == true)
            _animator.SetTrigger(reloadAnimationTriggername);
        _audioSource.PlayOneShot(weaponScriptableObject.reload);

        yield return new WaitForSeconds(weaponScriptableObject.reloadTime);

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


