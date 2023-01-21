using System.Collections;
using GameBase;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Random = UnityEngine.Random;

public enum States
{
    Active,
    Destroied
}    
public enum ShootType
{
    Auto,
    Semi,
    Burst
}

public enum PlaySoundType
{
    Shoot,
    Empty, 
    Reload
}


abstract public class Weapon : MonoBehaviour
{

    public WeaponScriptableObject weaponScriptableObject;

    [SerializeField] private GamePlayer gamePlayer;

    [Space]
    [Header("Enums")]
    [SerializeField] private States _state;

    [SerializeField] public ShootType _shootType;



    [Space(10)]
    [Header("Animation")]
    [SerializeField] protected Animator animator;

    [SerializeField] protected string shootAnimationName = "Shoot";
    [SerializeField] protected string reloadAnimationName = "Reload";

    [Space(10)]
    [Header("Audio")]
    [SerializeField] protected bool useAudio;

    [Space(10)] 
    [Header("Sway")] 
    [SerializeField] private bool canSway;
    [SerializeField] private Vector3 initialWeaponPosition;
    [SerializeField] private Vector3 initialSwayPosition;

    [SerializeField] private float swayMultiplier;
    [SerializeField] private float maxSwayDistance;
    [SerializeField] private float swaySmoothing;

    private PlayerControls controls;
    private Vector2 lookVector;

    //[Space(10)]
    //[Header("Aiming")]
    //[SerializeField] private bool canScope;
    //[SerializeField] protected Attachment attachment;
    //[SerializeField] private float normalFOV;
    //[SerializeField] private float scopedFOV;
    //[SerializeField] private float normalSens;
    //[SerializeField] private float scopedSens;
    //[SerializeField] private float FOVSmoothing;
    //[SerializeField] private Vector3 normalLOcalPosition;
    //[SerializeField] private Vector3 aimingLOcalPosition;
    //[SerializeField] private float aimSmoothing;
    //[SerializeField] private string ammoObjectName;

    [Space(10)]
    [Header("Ammo")]
    [SerializeField] public WeaponAmmo weaponAmmo;
    [SerializeField] public int ammo;
    [SerializeField] private int maxAmmo;

    [SerializeField] private bool hasInfiniteAmmo;
    [SerializeField] private int reserveAmmo;

    [Space(10)]
    [Header("Effects")]
    [SerializeField] private Transform muzzlePosition;


    [Space(10)]
    [Header("Layers")]
    public LayerMask hitMask;

    public GameObject player;
    public bool canShoot;


    public Camera cam;
    public Camera gunCam;
    public AudioSource audioSource;
    private TMP_Text ammoText;

    [SerializeField] private Transform weaponView;

    public bool reloading { get; private set; } = false;

#region AbstractVariables

    public abstract float nextFire { get; }

#endregion



#region AbstractMethods

    public abstract Ray Shoot();
    public abstract void FireButtonWasReleased();

#endregion

    [field: SerializeField] public bool _isLocalPLayer { get; set; }


    private void Start()
    {
        if (!_isLocalPLayer)
        {
            weaponView.gameObject.layer = LayerMask.NameToLayer("Default");
            for (int i = 0; i < weaponView.childCount; i++)
            {
                weaponView.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
            }
            return;
        }

        gamePlayer.GetComponent<Health>().onDeath += OnDeath;
        controls = new PlayerControls();
        controls.Player.Enable();
        controls.Player.Look.performed += ReadLookVector;

        initialWeaponPosition = transform.position;

        cam = Camera.main;
        gunCam = cam.GetComponentInChildren<Camera>();  
        GameObject help = Instantiate(weaponAmmo.gameObject);


        weaponAmmo = help.GetComponent<WeaponAmmo>();

        weaponAmmo.Ammo = ammo;
        weaponAmmo.ClipSize = maxAmmo;
        weaponAmmo.ReserveAmmo = reserveAmmo;

        weaponAmmo.AmmoText = ammoText;
        canShoot = true;

    }

    //private void Update()
    //{
    //    Sway();
    //}

    protected Ray GetRay()
    {
        return new Ray(cam.transform.position, cam.transform.forward);
    }

    public IEnumerator ReloadCoroutine()
    {
        canShoot = false;
        reloading = true;

        animator.Play(reloadAnimationName);

        yield return new WaitForSeconds(weaponScriptableObject.reloadTime);

        weaponAmmo.AddAmmo();
        weaponAmmo.UpdateAmmoInScreen();
        canShoot = true;
        reloading = false;
    }


    private void ReadLookVector(InputAction.CallbackContext context) => lookVector = context.ReadValue<Vector2>();

    private void Sway()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, lookVector * swayMultiplier, swaySmoothing * Time.deltaTime);



    }

    public void PlaySound(PlaySoundType soundType)
    {
        switch (soundType)
        {
            case PlaySoundType.Empty:
                audioSource.PlayOneShot(weaponScriptableObject.empty);
                break;

            case PlaySoundType.Shoot:
                audioSource.PlayOneShot(weaponScriptableObject.shotSounds[Random.Range(0, weaponScriptableObject.shotSounds.Length-1)]);
                break;

            case PlaySoundType.Reload:
                print("reload");
                audioSource.PlayOneShot(weaponScriptableObject.reload);
                break;
        }

    }

    protected void SpawnMuzzle()
    {
        if (weaponScriptableObject.muzzleFlash is not null)
        {
            Transform spawnedMuzzle = Instantiate(weaponScriptableObject.muzzleFlash, muzzlePosition.position, muzzlePosition.rotation, muzzlePosition);

            Destroy(spawnedMuzzle.gameObject, weaponScriptableObject.muzzleFlashLifetime);
        }
    }

    public void OnDeath(string str)
    {
        weaponAmmo.Ammo = weaponAmmo.ClipSize;
        weaponAmmo.UpdateAmmoInScreen();
    }

}

//[System.Serializable]
//public class Attachment
//{
//    [Header("Silencer")]
//    public bool hasSilencer;
//    public GameObject silencerObject;

//    [Header("Scope`s")]
//    public int Scopeid;
//    public GameObject ironSight;

//    [Space(10)]
//    public AttachmentInfo[] scopes;
//    public Vector3[] positionsInScope;
//}

