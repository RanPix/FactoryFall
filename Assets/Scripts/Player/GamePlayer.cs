using UnityEngine;
using System.Collections;
using Mirror;
using GameBase;
using Player.Info;


namespace Player
{
    public class GamePlayer : NetworkBehaviour
    {
        private PlayerInfo playerInfo;

        [SerializeField] private Health health;
        [SerializeField] private Damage damage;

        [SerializeField] private InventoryUI inventory;
        [field: SerializeField]public GameObject cameraHolder { get; private set; }

        [SerializeField] private Transform cameraPosition;
        [SerializeField] private Transform orientation;

        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private GameObject ammoTextPrefab;

        [SerializeField] private Camera miniMapCamera;
        [SerializeField] private GameObject playerMark;


        [SerializeField] private GameObject compass;
        [SerializeField] private AudioSource audioSource;


        private Canvas canvas;
        private Transform cam;

        private void Start()
        {
            canvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<Canvas>();
            if (isLocalPlayer)
            {
                gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
                cameraHolder = Instantiate(cameraHolder);
                cameraHolder.GetComponent<MoveCamera>().cameraPosition = cameraPosition;
                cameraHolder.GetComponent<Look>().orientation = orientation;
                cameraHolder.GetComponent<Look>().inventoryUI = inventory;
                cameraHolder.GetComponent<Look>()._isLocalPlayer = true;
                cam = cameraHolder.GetComponentInChildren<Camera>().transform;

                GetComponent<SyncRotation>().reference = cam;

                Camera _miniMapCamera = Instantiate(miniMapCamera);
                GameObject playerRow = GameObject.Instantiate(playerMark);
                PlayerMark _playerMark = playerRow.GetComponent<PlayerMark>();
                _playerMark.player = gameObject.transform;
                _playerMark.isLocal = true;
                _playerMark.rotationReference = gameObject.transform.GetChild(0).GetChild(0);
                MiniMapCameraMove _miniMapCameraMove = _miniMapCamera.GetComponent<MiniMapCameraMove>();
                _miniMapCameraMove.player = gameObject.transform;

                GameObject _compass = GameObject.Instantiate(compass, canvas.transform);
                _compass.GetComponent<Compass>().reference = gameObject.transform.GetChild(0).GetChild(0);
                health.onDeath += OnDeath;


                GameObject healthBar = Instantiate(healthBarPrefab, GameObject.Find("Canvas").transform);
                healthBar.GetComponent<HealthBar>().playerHealth = GetComponent<Health>();
            }
            else
            {
                GameObject playerRow = GameObject.Instantiate(playerMark);
                PlayerMark _playerMark = playerRow.GetComponent<PlayerMark>();
                _playerMark.player = gameObject.transform;
                _playerMark.isLocal = false;
                _playerMark.rotationReference = gameObject.transform.GetChild(0).GetChild(0);

                //enabled = false;
            }
            /*if (!isLocalPlayer)
            {
                cameraHolder.GetComponentInChildren<Camera>().enabled = false;
                cameraHolder.GetComponentInChildren<Camera>().gameObject.GetComponentInChildren<Camera>().enabled = false;

            }*/

            //NetworkServer.Spawn(cameraHolder);
            //PlayerInteraction.instance.player = this;
        }

        private void Update()
        {
            if(!isLocalPlayer)
                return;/*
            Ray ray = new Ray(cam.position, cam.forward);
            Shoot(ray);*/
        }

        private void OnDeath()
        {
            print("ded");
            /*cameraHolder.SetActive(false);
            gameObject.SetActive(false);*/
        }

        /*public void Interact(IInteractable interact) =>
            InteractServer(interact);

                //PlayerInteraction.instance.player = this;
            }

            /*public void Interact(IInteractable interact) =>
                InteractServer(interact);

            [Command]
            private void InteractServer(IInteractable interact) =>
                interact.Interact();



            public void RemoveBlock(Block block) =>
                RemoveBlockServer(block);

            [Command]
            private void RemoveBlockServer(Block block) =>
                block.RemoveBlock();*/

        public void Shoot(Ray ray, LayerMask mask, float damage, float shootRange)
        {
            ShootServer(ray, mask, damage, shootRange);
        }

        /*public void PlayAudio(AudioClip clip)
        {
            PlayAudioOnClients(clip);
        }

        [ClientRpc]
        private void PlayAudioOnClients(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }*/
        [Command]
        private void ShootServer(Ray ray, LayerMask mask, float damage, float shootRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, shootRange))
            {
                Health hitHealth = hit.transform.GetComponent<Health>();
                if (hitHealth)
                {
                    hitHealth.gotDamage = damage;
                    hitHealth.Damage();
                }
            }
        }
    }
}