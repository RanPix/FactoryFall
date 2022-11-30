using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
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

abstract public class Weapon : MonoBehaviour
{

    [SerializeField] protected WeaponScriptableObject weaponScriptableObject;

    [Space]
    [Header("Enums")]
    [SerializeField] private States _state;

    [SerializeField] private ShootType _shootType;

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
    [SerializeField] protected WeaponAmmo weaponAmmo;
    [SerializeField] private int ammo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int reserveAmmo;

    [Space(10)]
    [Header("Muzzle")]
    [SerializeField] protected Transform muzzlePosition;
    [SerializeField] protected GameObject[] muzzle;
    [Space(10)]
    [SerializeField] protected GameObject bulletSpawner;
    [SerializeField] private ConnectorHelper connectorHelper;
    [Space(15)]
    [Header("Events")]
    private Action OnInScopeValuseChange;
    public Action OnFireButtonPressed;
    public Action OnFireButtonPerformed;
    public Action OnFireButtonReleased;
    public Action OnReloadButtonPerformed;
    public Action OnScopeButtonPressed;
    //public Action OnFireButton;

    [Space(10)]
    public bool canShoot;

    //protected float _nextFire;
    private PlayerControls controls;
    protected Camera cam;
    private Camera gunCam;
    private AudioSource audioSource;
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
    protected abstract float nextFire { get; }
    #endregion
    #region AbstractMethods
    public abstract void Shoot();
    public abstract void Scope();
    protected abstract void SpawnBullet();
    protected abstract void FireButtonWasReleased();
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
        audioSource = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
    }
    private void Start()
    {
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

        #region Events

        OnFireButtonPressed += Shoot;
        OnFireButtonPerformed += Shoot;
        OnReloadButtonPerformed += Reload;


        #endregion

    }
    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case States.Active:

                Aiming();
                KeyCodes();
                break;
        }

    }
    
    protected void RayCasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weaponScriptableObject.weaponShootRange, playerMask))
        {

        }
    }
    private void KeyCodes()
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
                        OnFireButtonPressed?.Invoke();
                    }
                    else if (_shootType == ShootType.Semi && controls.Player.Fire.WasPerformedThisFrame())
                    {
                        Shoot();
                        OnFireButtonPerformed?.Invoke();
                    }
                }
            }
        }
        if (_shootType == ShootType.Auto && controls.Player.Fire.WasReleasedThisFrame())
        {
            FireButtonWasReleased(); 
            OnFireButtonReleased?.Invoke();
        }
        if (controls.Player.Fire.IsPressed() && weaponAmmo.Ammo <= 0)
        {
            audioSource.PlayOneShot(weaponScriptableObject.empty);
        }
        if (controls.Player.Reload.WasPerformedThisFrame())
        {
            if (weaponAmmo.ReserveAmmo > 0 && weaponAmmo.Ammo < ammo)
            {
                OnReloadButtonPerformed?.Invoke();
                //StartCoroutine(ReloadCoroutine());
            }
        }
    }
    private IEnumerator ReloadCoroutine()
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

    private void Reload()
    {
        StartCoroutine(ReloadCoroutine());
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

