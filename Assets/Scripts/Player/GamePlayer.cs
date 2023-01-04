using UnityEngine;
using System.Collections;
using Mirror;
using GameBase;
using Player.Info;


namespace Player
{
    [RequireComponent(typeof(PlayerSetup))]
    public class GamePlayer : NetworkBehaviour
    {
        private PlayerInfo playerInfo;

        [SerializeField] private Health health;

        [field: SyncVar] public bool isDead { get; private set; }

        [SerializeField] private Behaviour[] disableOnDeath;
        private bool[] wasEnabled;

        [SerializeField] private GameObject[] disableGameObjectsOnDeath;

        /*[SerializeField] private GameObject deathEffect;
        [SerializeField] private GameObject spawnEffect;*/
        
        private bool firstSetup = true;

        [SerializeField] private InventoryUI inventory;
        [field: SerializeField] public GameObject cameraHolder { get; private set; }

        [SerializeField] private Transform cameraPosition;
        [SerializeField] private Transform orientation;

        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private GameObject ammoTextPrefab;

        [SerializeField] private Camera miniMapCamera;
        [SerializeField] private GameObject playerMark;


        [SerializeField] private GameObject compass;
        [SerializeField] private AudioSource audioSource;



        private GameManager gameManager;
        private Canvas canvas;
        private Transform cam;
        private bool useEffects;

        private void SetupCameraHolder()
        {
            cameraHolder = Instantiate(cameraHolder);
            cameraHolder.GetComponent<MoveCamera>().cameraPosition = cameraPosition;
            cameraHolder.GetComponent<Look>().orientation = orientation;
            cameraHolder.GetComponent<Look>()._isLocalPlayer = true;
            cam = cameraHolder.GetComponentInChildren<Camera>().transform;

            GetComponent<SyncRotation>().reference = cam;
        }

        private void SetupMiniMap()
        {
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

        }

        private void Start()
        {
            canvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<Canvas>();
            if (isLocalPlayer)
            {
                Debug.Log("NetID222222" + GetComponent<NetworkIdentity>().netId.ToString());
                playerInfo = new PlayerInfo(null, Team.None, GetComponent<NetworkIdentity>().netId.ToString(), transform.name);
                

                SetupCameraHolder();
                SetupMiniMap();
                
                health.onDeath += Die;

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

            }

        }

        public void SetupPlayer()
        {
            if (isLocalPlayer)
            {
                //Switch cameras
                GameManager.instance.SetSceneCameraActive(false);
            }

            CmdBroadCastNewPlayerSetup();
        }

        [Command]
        private void CmdBroadCastNewPlayerSetup()
        {
            RpcSetupPlayerOnAllClients();
        }

        [ClientRpc]
        private void RpcSetupPlayerOnAllClients()
        {
            if (firstSetup)
            {
                wasEnabled = new bool[disableOnDeath.Length];
                for (int i = 0; i < wasEnabled.Length; i++)
                {
                    wasEnabled[i] = disableOnDeath[i].enabled;
                }

                firstSetup = false;
            }

            SetDefaults();
        }


        private void Die(string _sourceID)
        {
            print($"_sourceID = {_sourceID}");
            isDead = true;

            PlayerInfo sourcePlayer = GameManager.GetPlayer(_sourceID);

            if (sourcePlayer != null)
            {
                Debug.Log("DIE111111111111");
                sourcePlayer.kills++;
                GameManager.instance.OnPlayerKilledCallback.Invoke(playerInfo.netID, sourcePlayer.name);
            }

            playerInfo.deaths++;

            Debug.Log("DIE2222222222");
            //Disable components
            for (int i = 0; i < disableOnDeath.Length; i++)
                disableOnDeath[i].enabled = false;

            //Disable GameObjects
            for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
                disableGameObjectsOnDeath[i].SetActive(false);

            //Spawn a death effect
            /*GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(_gfxIns, 3f);*/

            //Switch cameras
            if (isLocalPlayer)
                GameManager.instance.SetSceneCameraActive(true);

            Debug.Log(transform.name + " is DEAD!");

            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            Debug.Log("Res1111111");
            yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

            Transform _spawnPoint = SpawnPoints.instance.GetSpawnPoint();
            transform.position = _spawnPoint.position;
            transform.rotation = _spawnPoint.rotation;

            yield return new WaitForSeconds(0.1f);

            SetupPlayer();

            Debug.Log(transform.name + " respawned.");
        }

        public void SetDefaults()
        {
            isDead = false;

            health.SetHealth(health.maxHealth);

            //Enable the components
            for (int i = 0; i < disableOnDeath.Length; i++)
                disableOnDeath[i].enabled = wasEnabled[i];

            //Enable the gameobjects
            for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
                disableGameObjectsOnDeath[i].SetActive(true);


            //Create spawn effect
            /*GameObject _gfxIns = Instantiate(spawnEffect, transform.position, Quaternion.identity);
            Destroy(_gfxIns, 3f);*/
        }


        public void Shoot(Ray ray, LayerMask mask, float damage, float shootRange, string playerID)
        {
            ShootServer(ray, mask, damage, shootRange, playerID);
        }

        [Command]
        private void ShootServer(Ray ray, LayerMask mask, float damage, float shootRange, string playerID)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, shootRange))
            {
                Health hitHealth = hit.transform.GetComponent<Health>();
                if (hitHealth)
                {
                    hitHealth.gotDamage = damage * GameManager.instance.matchSettings.dmgMultiplier;
                    hitHealth.Damage(playerID);
                }
            }
        }

        public string GetLocalNetID() => playerInfo.netID;
        public PlayerInfo GetPlayerInfo() => playerInfo;
    }
}