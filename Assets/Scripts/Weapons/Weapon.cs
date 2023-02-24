using System.Collections;
using System.Collections.Generic;
using GameBase;
using Mirror;
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

public enum WeaponName
{
    Pistol,
    AUG,
    Shotgun,
}
public enum WeaponType
{
    Pistol,
    AutomaticRifle,
    Shotgun,

}
[RequireComponent(typeof(WeaponRecoil))]
public class Weapon : MonoBehaviour
{

    public WeaponScriptableObject weaponScriptableObject;

    [SerializeField] private GamePlayer gamePlayer;

    [Space] 

    [Header("Enums")] 
    [SerializeField] private States _state;

    [field: SerializeField] public ShootType shootType { get; private set; }
    [field: SerializeField] public WeaponName weaponName{get; private set; }
    [field: SerializeField] public WeaponType weaponType{get; private set; }



    [field:Space(10)] 

    [field: Header("Animation")] 

    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public string shootAnimationName { get; private set; } = "Shoot";
    [field: SerializeField] public string reloadAnimationName { get; private set; } = "Reload";



    [Space(10)] 

    [Header("Audio")] 
    [SerializeField] protected bool useAudio;
    private AudioSync audioSync;


    [Space(10)] 

    [Header("Sway")] 
    public Vector3 initialWeaponPosition;

    [Space(10)] 

    [Header("Recoil")] 
    public WeaponRecoil recoil;

    [Space]

    private PlayerControls controls;
    private Vector2 lookVector;


    [Space(10)] 

    [Header("Ammo")]
    public WeaponAmmo weaponAmmo;

    public float shootTimer { get; private set; }

    [field: SerializeField] public bool hasInfiniteAmmo { get; private set; }
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
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Transform weaponView;

    public bool reloading { get; private set; } = false;
    public float nextFire { get; private set; }



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

        audioSync = NetworkClient.localPlayer.GetComponent<AudioSync>();


        weaponAmmo = Instantiate(weaponAmmo.gameObject).GetComponent<WeaponAmmo>();

        UpdateAmmo();


        canShoot = true;

    }

    private void OnDestroy()
    {
        gamePlayer.GetComponent<Health>().onDeath -= OnDeath;
        controls.Player.Look.performed -= ReadLookVector;
    }

    private void Update()
    {
        shootTimer+=Time.deltaTime;
    }

    public Ray[] Shoot()
    {
        shootTimer = 0;
        nextFire = Time.time;
        SpawnMuzzle();
        if (weaponScriptableObject.useOneAmmoPerShot)
        {
            animator.Play(shootAnimationName);
            weaponAmmo.Ammo--;
            weaponAmmo.UpdateAmmoInScreen();
            recoil.RecoilFire();
        }


        return GetRay(weaponScriptableObject.patern);
    }



public void UpdateAmmo()
    {
        weaponAmmo.Ammo = weaponScriptableObject.maxAmmo;
        weaponAmmo.ClipSize = weaponScriptableObject.maxAmmo;
        weaponAmmo.ReserveAmmo = reserveAmmo;

        weaponAmmo.AmmoText = ammoText;
        weaponAmmo.UpdateAmmoInScreen();
    }

    /*void OnEnable()
    {
        UpdateAmmo();
    }*/

    private Ray[] GetRay(Vector2[] pattern)
    {
        Ray[] rays = new Ray[pattern.Length];
        for (int i = 0; i < pattern.Length; i++)
        {
            rays[i] = (new Ray(cam.transform.position, new Vector3(cam.transform.forward.x+pattern[i].x, cam.transform.forward.y + pattern[i].y, cam.transform.forward.z)));
        }
        return rays;
    }

    public IEnumerator ReloadCoroutine()
    {
        bool isFirstIteration = true;
        do
        {
            canShoot = false;
            reloading = true;

            animator.Play(reloadAnimationName);

            audioSync.PlaySound(ClipType.weapon,  false, $"{weaponName}_Reload");

            yield return new WaitForSeconds(weaponScriptableObject.reloadTime);

            if (!wasChanged && weaponType != WeaponType.Shotgun)
            {
                weaponAmmo.ResetAmmo();
                weaponAmmo.UpdateAmmoInScreen();

            }
            else if (!wasChanged && weaponType == WeaponType.Shotgun)
            {
                weaponAmmo.AddAmmo(1);
                weaponAmmo.UpdateAmmoInScreen();
            }
            canShoot = true;
            reloading = false;
            isFirstIteration = false;

        } while (!wasChanged && weaponType == WeaponType.Shotgun && weaponAmmo.Ammo < weaponScriptableObject.maxAmmo);
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
    }

}


