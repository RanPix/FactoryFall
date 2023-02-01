using System.Collections;
using System.Collections.Generic;
using GameBase;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public Vector3 initialWeaponPosition;

    private PlayerControls controls;
    private Vector2 lookVector;

    [Space(10)]
    [Header("Ammo")]
    [SerializeField] public WeaponAmmo weaponAmmo;
    [SerializeField] public int ammo;
    [SerializeField] private int maxAmmo;
    [SerializeField] public int numberOfBulletsPerShot;
    [SerializeField] public float timeBetweenSpawnBullets;

    [SerializeField] protected bool hasInfiniteAmmo;
    [SerializeField] protected int reserveAmmo;

    [Space(10)] 
    [Header("Effects")]
    public Transform muzzlePosition;


    [Space(10)]
    [Header("Layers")]
    public LayerMask hitMask;

    public GameObject player;
    public bool canShoot;
    public bool wasChanged = false;
    public int weaponIndex;


    public Camera cam;
    public Camera gunCam;
    public AudioSource audioSource;
    [SerializeField]private TMP_Text ammoText;
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

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject == gameObject)
            {
                weaponIndex = i;
                break;
            }
        }

        ammoText = CanvasInstance.instance.weaponAmmoText.GetComponent<TMP_Text>();

        gamePlayer.GetComponent<Health>().onDeath += OnDeath;
        controls = new PlayerControls();
        controls.Player.Enable();
        controls.Player.Look.performed += ReadLookVector;

        cam = Camera.main;
        gunCam = cam.GetComponentInChildren<Camera>();


        GameObject help = Instantiate(weaponAmmo.gameObject);


        weaponAmmo = help.GetComponent<WeaponAmmo>();
        /*weaponAmmo.Ammo = ammo;
        weaponAmmo.ClipSize = maxAmmo;
        weaponAmmo.ReserveAmmo = reserveAmmo;

        weaponAmmo.AmmoText = ammoText;*/ // :skull:
        UpdateAmmo();


        canShoot = true;

    }

    public void UpdateAmmo()
    {
        weaponAmmo.Ammo = ammo;
        weaponAmmo.ClipSize = maxAmmo;
        weaponAmmo.ReserveAmmo = reserveAmmo;

        weaponAmmo.AmmoText = ammoText;
        weaponAmmo.UpdateAmmoInScreen();
    }

    /*void OnEnable()
    {
        UpdateAmmo();
    }*/

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
        if (!wasChanged)
        {
            weaponAmmo.AddAmmo();
            weaponAmmo.UpdateAmmoInScreen();
        }
        canShoot = true;
        reloading = false;
    }

    private void ReadLookVector(InputAction.CallbackContext context) => lookVector = context.ReadValue<Vector2>();



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

