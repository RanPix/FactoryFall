using System;
using System.Collections;
using Mirror;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
public enum States
{
    Active,
    Destroied
}    
public enum ShootType
{
    Auto,
    Semi
}
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]

abstract public class Weapon : NetworkBehaviour
{

    [SerializeField] public WeaponScriptableObject weaponScriptableObject;

    [Space]
    [Header("Enums")]
    [SerializeField] private States _state;

    [SerializeField] public ShootType _shootType;

    [Space(10)]

    [SerializeField] protected LayerMask playerMask;

    [Space(10)]
    [Header("Animation")]
    [SerializeField] protected bool useAnimations;
    [SerializeField] protected Animator animator;
    [SerializeField] protected string shootAnimationName;
    [SerializeField] private string reloadAnimationTriggername;
    [Space(10)]
    [Header("Audio")]
    [SerializeField] protected bool useAudio;
    [Space(10)]
    [Header("Sway")]
    [SerializeField] private bool canSway;
    [SerializeField] private Vector3 initialSwayPosition;
    [SerializeField] private float SwayAmount;
    [SerializeField] private float MaxSwayAmount;
    [SerializeField] private float swaySmoothing;

    [Space(10)]
    [Header("Aiming")]
    [SerializeField] private bool canScope;
    [SerializeField] protected Attachment attachment;
    [SerializeField] private float normalFOV;
    [SerializeField] private float scopedFOV;
    [SerializeField] private float normalSens;
    [SerializeField] private float scopedSens;
    [SerializeField] private float FOVSmoothing;
    [SerializeField] private Vector3 normalLOcalPosition;
    [SerializeField] private Vector3 aimingLOcalPosition;
    [SerializeField] private float aimSmoothing;
    [SerializeField] private string ammoObjectName;

    [Space(10)]
    [Header("Ammo")]
    [SerializeField] public WeaponAmmo weaponAmmo;
    [SerializeField] public int ammo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int reserveAmmo;

    [Space(10)]
    [Header("Muzzle")]
    [SerializeField] protected Transform muzzlePosition;
    [SerializeField] protected GameObject[] muzzle;
    [Space(10)]
    [SerializeField] private GameObject bulletSpawner;
    [SerializeField] private ConnectorHelper connectorHelper;
    [SerializeField] private Action OnInScopeValuseChange;

    public GameObject player;
    public bool canShoot;
    [field: SerializeField] public bool _isLocalPlayer { get; set; } = false;
    public PlayerControls controls { get; private set; }



    //protected float nextFire;
    public Camera cam;
    public Camera gunCam;
    public AudioSource audioSource;
    private TMP_Text ammoText;

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
    #region AbstractVariables
    public abstract float nextFire { get; }
    #endregion
    #region AbstractMethods
    public abstract void Shoot();
    public abstract void Scope();
    public abstract void FireButtonWasReleased();
    #endregion


    // Start is called before the first frame update
    private void Start()
    {
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
        animator = gameObject.GetComponentInChildren<Animator>();
        controls = new PlayerControls();
        controls.Enable();
        if(!_isLocalPlayer)
            return;
        WeaponsLink.instance.weapons.Add(this);
        cam = Camera.main;
        gunCam = cam.GetComponentInChildren<Camera>();  
        GameObject help = Instantiate(weaponAmmo.gameObject);

        weaponAmmo = help.GetComponent<WeaponAmmo>();
        canShoot = true;
        weaponAmmo.Ammo = ammo;
        weaponAmmo.ClipSize = maxAmmo;
        weaponAmmo.ReserveAmmo = reserveAmmo;
        weaponAmmo.AmmoText = ammoText;

    }
    // Update is called once per frame
    void Update()
    {

    }
    [Command]
    protected void RayCasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weaponScriptableObject.weaponShootRange, playerMask))
        {

        }
    }
    [Command]
    protected /*IEnumerator*/ void SpawnBullet()
    {
        GameObject spawnedBullet = NetworkManager.Instantiate(weaponScriptableObject.bulletPrefab);
        spawnedBullet.transform.position = bulletSpawner.transform.position;
        spawnedBullet.GetComponent<Bullet>().AddForceBullet(bulletSpawner.transform.forward * weaponScriptableObject.bulletSpeed);
        NetworkServer.Spawn(spawnedBullet);
        /*yield return new WaitForSeconds(weaponScriptableObject.bulletTimeToDestroy);*/
        NetworkServer.Destroy(spawnedBullet);

    }
    /*public void KeyCodes()
    {
        if (canShoot == true)
        {
            if (weaponAmmo.Ammo > 0)
            {
                        
                if (Time.time - nextFire > 1 / weaponScriptableObject.fireRate)
                {
                    if (_shootType == ShootType.Auto && controls.Player.Fire.IsPressed())
                    {
                        Shoot();
                    }
                    else if (_shootType == ShootType.Semi && controls.Player.Fire.WasPerformedThisFrame())
                    {
                        Shoot();

                    }
                    else if (_shootType == ShootType.Auto && controls.Player.Fire.WasReleasedThisFrame())
                    {
                        FireButtonWasReleased();
                    }
                }
            }
        }
        if (controls.Player.Fire.IsPressed() && weaponAmmo.Ammo <= 0)
        {
            audioSource.PlayOneShot(weaponScriptableObject.empty);
        }
        if (controls.Player.Reload.WasPerformedThisFrame())
        {
            if (weaponAmmo.ReserveAmmo > 0 && weaponAmmo.Ammo < ammo)
            {
                StartCoroutine(ReloadCoroutine());
            }
        }

    }*/
    public IEnumerator ReloadCoroutine()
    {
        canShoot = false;
        if (useAnimations == true)
            animator.SetTrigger(reloadAnimationTriggername);
        if(useAudio)
            audioSource.PlayOneShot(weaponScriptableObject.reload);

        yield return new WaitForSeconds(weaponScriptableObject.reloadTime);

        weaponAmmo.AddAmmo();
        weaponAmmo.ApdateAmmoInScreen();
        canShoot = true;
    }

    private void Aiming()
    {
        /*Vector3 target = normalLOcalPosition;

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
        transform.localPosition = desiredPosition;*/
    }
    public void PlayShootSound()
    {
        AudioClip clip = null;
        if (attachment.haveSilencer == true)
        {
            clip = weaponScriptableObject.shootsSilencer[Random.Range(0, weaponScriptableObject.shoots.Length)];
            audioSource.clip = clip;
            audioSource.Play();

        }
        else if (attachment.haveSilencer == false)
        {
            clip = weaponScriptableObject.shoots[Random.Range(0, weaponScriptableObject.shoots.Length)];
            audioSource.clip = clip;
            audioSource.Play();

        }
    }
    public void SpawmMuzzle()
    {
        if (attachment.haveSilencer == false)
        {
            if (weaponScriptableObject.haveMuzzle == true)
            {
                GameObject currentMuzzle = muzzle[Random.Range(0, muzzle.Length)];
                GameObject spawnedMuzzle = Instantiate(currentMuzzle, muzzlePosition.position, muzzlePosition.rotation);
                spawnedMuzzle.transform.localScale = new Vector3(weaponScriptableObject.scaleFactor, weaponScriptableObject.scaleFactor, weaponScriptableObject.scaleFactor);
                Destroy(spawnedMuzzle, weaponScriptableObject.TimeTodestroy);
            }
        }
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

