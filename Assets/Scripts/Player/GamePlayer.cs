using UnityEngine;
using System.Collections;
using Mirror;
using GameBase;
using Player.Info;
using System;
using UI.Indicators;

namespace Player//https://www.youtube.com/watch?v=dQw4w9WgXcQ
{
    [RequireComponent(typeof(PlayerSetup))]
    public class GamePlayer : NetworkBehaviour
    {
        private PlayerInfo playerInfo;

        [SerializeField] private Health health;

        [SerializeField] private GameObject nameGO;

        [field: SyncVar] public bool isDead { get; private set; }

        [SerializeField] private Behaviour[] disableOnDeath;
        private bool[] wasEnabled;

        [SerializeField] private GameObject[] disableGameObjectsOnDeath;
        [SerializeField] private CharacterController characterController;

        [SerializeField] private LayerMask hitMask;

        /*[SerializeField] private GameObject deathEffect;
        [SerializeField] private GameObject spawnEffect;*/

        private bool firstSetup = true;

        [SerializeField] private InventoryUI inventory;
        [field: SerializeField] public GameObject cameraHolder { get; private set; }

        [SerializeField] private Transform cameraPosition;
        [SerializeField] private Transform orientation;

        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private GameObject ammoTextPrefab;
        [SerializeField] private GameObject menuPrefab;

        [SerializeField] private Camera miniMapCamera;
        [SerializeField] private GameObject playerMark;


        [SerializeField] private GameObject compass;
        [SerializeField] private Transform killerPlayerInfoPrefab;
 
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private WeaponKeyCodes weaponKeyCodes;

        [SerializeField] private Transform muzzlePosition;

        [SerializeField] private Transform trail;

        [SerializeField] private Transform hitIndicatorPrefab;

        private int spawnedBulletCount = 0;

        private Canvas canvas;
        private Transform cam;
        private GameObject hitMarker;


        public Action<string, int> OnGotHit;

        public Action<string, string, int> OnDeath;
        public Action OnRespawn;

        public Action<string, string> OnKill;


        private void SetupCameraHolder()
        {
            cameraHolder = Instantiate(cameraHolder);
            cameraHolder.GetComponent<MoveCamera>().cameraPosition = cameraPosition;

            Look playerLook = cameraHolder.GetComponent<Look>();
            playerLook.orientation = orientation;
            playerLook._isLocalPlayer = true;
            playerLook.SetupEvents(this);

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
            playerInfo = new PlayerInfo(null, Team.Blue, GetComponent<NetworkIdentity>().netId.ToString(), gameObject.name);
            
            if (isLocalPlayer)
            {
                gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
                gameObject.tag = "LocalPlayer";
                canvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<Canvas>();

                for (int i = 0; i < canvas.transform.childCount; i++)
                {
                    if (canvas.transform.GetChild(i).name == "HitMarker")
                    {
                        hitMarker = canvas.transform.GetChild(i).gameObject;
                        break;
                    }
                }

                Transform hitIndicator = Instantiate(hitIndicatorPrefab, canvas.transform);
                hitIndicator.GetComponent<HitIndicatorTrigger>().Setup(this, orientation);

                SetupCameraHolder();
                SetupMiniMap();

                health.onDeath += Die;

                GameObject healthBar = Instantiate(healthBarPrefab, canvas.transform);
                healthBar.GetComponent<HealthBar>().playerHealth = GetComponent<Health>();

                GameObject menu = Instantiate(menuPrefab, canvas.transform);
                menu.GetComponent<Menu>().look = cameraHolder.GetComponent<Look>();

                Instantiate(killerPlayerInfoPrefab, canvas.transform).GetComponent<KillerPlayerInfo>().Setup(this);
            }
            else
            {
                GameObject playerRow = GameObject.Instantiate(playerMark);
                PlayerMark _playerMark = playerRow.GetComponent<PlayerMark>();
                _playerMark.player = gameObject.transform;
                _playerMark.isLocal = false;
                _playerMark.rotationReference = gameObject.transform.GetChild(0).GetChild(0);

                //nameGO.SetActive(true);
                //nameGO.GetComponentInChildren<TMP_Text>().text = GameManager.GetPlayer(GetComponent<NetworkIdentity>().netId.ToString()).playerInfo.name;
                this.enabled = false;
            }
        }


        
        

#region Weapon


        [Client]
        public void Shoot(Ray ray, int damage, float shootRange, string playerID)
        {
            spawnedBulletCount++;
            bool isHitted = Physics.Raycast(ray, out RaycastHit hit, shootRange, hitMask);
            if (isHitted)
            {
                Health hitHealth = hit.transform.GetComponent<Health>();
                if (hitHealth)
                {
                    StartCoroutine(ActivateForSeconds(hitMarker, 0.15f));
                    CmdPlayerShot(hit.transform.GetComponent<NetworkIdentity>().netId.ToString(), damage, playerID);
                }
            }

            CmdSpawnTrail(isHitted, ray.origin, ray.direction, hit.point, shootRange);
        }

        [Client]
        public void Punch(Ray ray, int damage, float punchDistance, float punchRadius, LayerMask hitLM, string playerID)
        {
            bool isHitted = Physics.SphereCast(ray, punchRadius, out RaycastHit hit, punchDistance, hitLM);
            if (isHitted)
            {
                Health hitHealth = hit.transform.GetComponent<Health>();
                if (hitHealth)
                {
                    StartCoroutine(ActivateForSeconds(hitMarker, 0.5f));
                    CmdPlayerShot(hit.transform.GetComponent<NetworkIdentity>().netId.ToString(), damage, playerID);
                }
            }
        }

        [Command]
        public void CmdSpawnTrail(bool isHitted, Vector3 origin, Vector3 direction, Vector3 point, float shootRange)
        {
            RpcSpawnTrail(isHitted, origin, direction, point, shootRange);
        }

        [ClientRpc]
        private void RpcSpawnTrail(bool isHitted, Vector3 origin, Vector3 direction, Vector3 point, float shootRange)
        {
            Transform _trail = Instantiate(trail);

            LineRenderer line = _trail.GetComponent<LineRenderer>();


            line.SetPosition(0, muzzlePosition.position);
            Vector3 trailFinish = isHitted ? point : origin + direction * shootRange;
            line.SetPosition(1, trailFinish);
        }



        [Command]
        private void CmdPlayerShot(string _playerID, int _damage, string _sourceID)
        {

            GamePlayer _player = GameManager.GetPlayer(_playerID);
            _player.RpcTakeDamage(_damage, _sourceID);
        }

        [ClientRpc]
        public void RpcTakeDamage(int _amount, string _sourceID)
        {
            if (isDead)
                return;

            health.Damage(_sourceID, _amount);

            if (isLocalPlayer)
                OnGotHit?.Invoke(_sourceID, _amount);
        }

#endregion



#region Death/Spawn/Respawn


        private void Die(string _sourceID)
        {
            //print($"_sourceID = {_sourceID}");
            isDead = true;

            PlayerInfo sourcePlayer = GameManager.GetPlayer(_sourceID).GetPlayerInfo();

            if (sourcePlayer != null)
            {
                //Debug.Log("DIE111111111111");
                GamePlayer player = GameManager.GetPlayer(_sourceID);

                OnDeath?.Invoke(_sourceID, player.GetPlayerInfo().name, (int)player.gameObject.GetComponent<Health>().currentHealth);

                sourcePlayer.kills++;
                CmdDie(_sourceID);
            }

            playerInfo.deaths++;

            //Debug.Log("DIE2222222222");

            CmdDisableComponentsOnDeath();

            //Spawn a death effect
            /*GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(_gfxIns, 3f);*/

            //Switch cameras
            //if (isLocalPlayer)
            //    GameManager.instance.SetSceneCameraActive(true);


            StartCoroutine(Respawn());
        }

        [Command]
        private void CmdDie(string _sourceID)
        {

            RpcDie(_sourceID);
        }

        [ClientRpc]
        private void RpcDie(string _sourceID)
        {
            GamePlayer player = GameManager.GetPlayer(_sourceID);

            GameManager.instance.OnPlayerKilledCallback?.Invoke(playerInfo.netID, player.GetPlayerInfo().name, (int)player.gameObject.GetComponent<Health>().currentHealth);
        }

        [Command]
        private void CmdDisableComponentsOnDeath()
            => RpcDisableComponentsOnDeath();

        [ClientRpc]
        private void RpcDisableComponentsOnDeath()
        {

            //Disable components
            for (int i = 0; i < disableOnDeath.Length; i++)
                disableOnDeath[i].enabled = false;
            GetComponent<CharacterController>().enabled = false;


            //Disable GameObjects
            for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
                disableGameObjectsOnDeath[i].SetActive(false);

        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

            Transform _spawnPoint = NetworkManagerFF.GetRespawnPosition();
            transform.position = _spawnPoint.position;
            transform.rotation = _spawnPoint.rotation;

            OnRespawn.Invoke();

            yield return new WaitForSeconds(0.1f);

            SetupPlayer();
        }
        #endregion



#region Setup


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
                    wasEnabled[i] = disableOnDeath[i].enabled;


                for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
                    disableGameObjectsOnDeath[i].SetActive(true);

                firstSetup = false;
            }

            SetDefaults();
        }


        public void SetDefaults()
        {
            isDead = false;

            health.SetHealth(health.maxHealth);

            //Enable the components
            for (int i = 0; i < disableOnDeath.Length; i++)
                disableOnDeath[i].enabled = wasEnabled[i];
            GetComponent<CharacterController>().enabled = true;
            //Enable the gameobjects
            for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
                disableGameObjectsOnDeath[i].SetActive(true);


        }


#endregion

        private IEnumerator ActivateForSeconds(GameObject GO, float time)
        {
            GO.SetActive(true);

            //Vector3 startScale = GO.transform.localScale;
            //GO.transform.DOScale(startScale + new Vector3(3f, 3f, 3f), time);

            yield return new WaitForSeconds(time);

            GO.SetActive(false);
            //GO.transform.localScale = startScale;
        }


        public string GetLocalNetID() => playerInfo.netID;
        public PlayerInfo GetPlayerInfo() => playerInfo;
    }
}