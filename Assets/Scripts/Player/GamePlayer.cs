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
        [SerializeField] private GameObject cross;
        [SerializeField] private GameObject menu;

        [SerializeField] private Camera miniMapCamera;
        [SerializeField] private GameObject playerMark;

        [SerializeField] private LayerMask mask;

        private Transform cam;

        private void Start()
        {
            if (isLocalPlayer)
            {
                gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
                cameraHolder = Instantiate(cameraHolder);
                cameraHolder.GetComponent<MoveCamera>().cameraPosition = cameraPosition;
                cameraHolder.GetComponent<Look>().orientation = orientation;
                cameraHolder.GetComponent<Look>()._isLocalPlayer = true;

                cam = cameraHolder.GetComponentInChildren<Camera>().transform;

<<<<<<< HEAD
                GetComponent<SyncRotation>().reference = cam;

                Camera _miniMapCamera = Instantiate(miniMapCamera);
                GameObject playerRow = GameObject.Instantiate(playerMark);
                PlayerMark _playerMark = playerRow.GetComponent<PlayerMark>();
                _playerMark.player = gameObject.transform;
                _playerMark.isLocal = true;
                _playerMark.rotationReference = gameObject.transform.GetChild(0).GetChild(0);
                MiniMapCameraMove _miniMapCameraMove = _miniMapCamera.GetComponent<MiniMapCameraMove>();
                _miniMapCameraMove.player = gameObject.transform;
                health.onDeath += OnDeath;


                GameObject healthBar = Instantiate(healthBarPrefab, GameObject.Find("Canvas").transform);
=======
                NetworkServer.Spawn(cameraHolder);
                
                health.onDeath += OnDeath;

                Transform canvas = GameObject.Find("Canvas").transform;

                Instantiate(cross, canvas);

                GameObject healthBar = Instantiate(healthBarPrefab, canvas);
>>>>>>> WeaponsAndMobs
                healthBar.GetComponent<HealthBar>().playerHealth = GetComponent<Health>();

                GameObject menuOnScene = Instantiate(menu, canvas);
                menuOnScene.GetComponent<Menu>().look = cameraHolder.GetComponent<Look>();
            }
            else
            {
<<<<<<< HEAD
                GameObject playerRow = GameObject.Instantiate(playerMark);
                PlayerMark _playerMark = playerRow.GetComponent<PlayerMark>();
                _playerMark.player = gameObject.transform;
                _playerMark.isLocal = false;
                _playerMark.rotationReference = gameObject.transform.GetChild(0).GetChild(0);

                //enabled = false;
            }
=======
                cameraHolder.GetComponentInChildren<AudioListener>().enabled = false;
            }
>>>>>>> WeaponsAndMobs
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
            Ray ray = new Ray(cam.position, cam.forward);
            Shoot(ray);
        }

        private void OnDeath()
        {
<<<<<<< HEAD
            print("ded");
            /*cameraHolder.SetActive(false);
            gameObject.SetActive(false);*/
        }

=======
            StartCoroutine(Respawn());

            cameraHolder.SetActive(false);
            gameObject.SetActive(false);
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(2f);

            cameraHolder.SetActive(true);
            gameObject.SetActive(true);
        }

        [Command]
        public void SpawnOnServer()
        {
            NetworkServer.Spawn(cameraHolder);
        }

>>>>>>> WeaponsAndMobs
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