using System;
using System.Collections;
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

    [SerializeField] public WeaponScriptableObject weaponScriptableObject;

    [Space]
    [Header("Enums")]
    [SerializeField] private States _state;

    [SerializeField] public ShootType _shootType;



    [Space(10)]
    [Header("Animation")]
    [SerializeField] protected Animator animator;

    [SerializeField] protected string shootAnimationName;
    [SerializeField] protected string reloadAnimationName;

    [Space(10)]
    [Header("Audio")]
    [SerializeField] protected bool useAudio;

    [Space(10)] 
    [Header("Sway")] 
    [SerializeField] private bool canSway;
    [SerializeField] private Vector3 initialWeaponPosition;
    [SerializeField] private Vector3 initialSwayPosition;

    [SerializeField] private float SwayAmount;
    [SerializeField] private float MaxSwayAmount;
    [SerializeField] private float swaySmoothing;

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
    [SerializeField] private Transform trail;


    [Space(10)]
    [Header("Layers")]
    public LayerMask playerMask;

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

    // Start is called before the first frame update
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
        weaponAmmo.ApdateAmmoInScreen();
        canShoot = true;
        reloading = false;
    }

    public void PlaySound(PlaySoundType soundType)
    {
        switch (soundType)
        {
            case PlaySoundType.Empty:
                audioSource.PlayOneShot(weaponScriptableObject.empty);
                break;

            case PlaySoundType.Shoot:
                audioSource.PlayOneShot(weaponScriptableObject.shoots[Random.Range(0, weaponScriptableObject.shoots.Length)]);
                break;

            case PlaySoundType.Reload:
                audioSource.PlayOneShot(weaponScriptableObject.reload);
                break;
        }

    }
    protected void SpawmMuzzle()
    {
        if (weaponScriptableObject.muzzleFlash is not null)
        {
            Transform spawnedMuzzle = Instantiate(weaponScriptableObject.muzzleFlash, muzzlePosition.position, muzzlePosition.rotation, muzzlePosition);

            Destroy(spawnedMuzzle.gameObject, weaponScriptableObject.muzzleFlashDeathTimer);
        }
    }

    protected void SpawnTrail(Vector3 hitPoint)
    {
        Transform _trail = Instantiate(trail);
        LineRenderer line = _trail.GetComponent<LineRenderer>();
        line.SetPosition(0, muzzlePosition.position);
        line.SetPosition(1, hitPoint);
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

