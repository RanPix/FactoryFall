using UnityEngine;
using System.Collections;
using Mirror;
using GameBase;
using System;
using System.Collections.Generic;
using TMPro;
using UI.Indicators;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;

namespace Player
{
    [RequireComponent(typeof(PlayerSetup))]
    public class GamePlayer : NetworkBehaviour
    {
#region SynkVars
        [field: SyncVar(hook = nameof(SetTeam))] public Team team { get; private set; } = Team.Null;
        [field: SyncVar (hook = nameof(SetNickname))] public string nickname { get; private set; }

        [field: SyncVar] public int kills { get; private set; }
        [field: SyncVar] public int deaths { get; private set; }
        [field: SyncVar] public int score { get; private set; }
#endregion


        [Header("Health")]
        [SerializeField] private Health health;
        [SerializeField] private GameObject healthBarPrefab;


        [Space]

        [Header("Weapon")]
        [SerializeField] private GameObject ammoTextPrefab;
        [SerializeField] private Transform trail;
        [SerializeField] private Transform hitIndicatorPrefab;
        [SerializeField] private WeaponKeyCodes weaponKeyCodes;

        public Transform muzzlePosition;

        private int spawnedBulletCount = 0;


        [Space]

        [Header("UI")]
        [SerializeField] private GameObject nameGO;
        [SerializeField] private InventoryUI inventory;
        [SerializeField] private GameObject menuPrefab;


        [Space]

        [Header("Cameras")]
        [SerializeField] private Transform cameraPosition;
        [SerializeField] private Transform orientation;

        [SerializeField] private Camera miniMapCamera;
        [SerializeField] private GameObject playerMark;
        [field: SerializeField] public GameObject cameraHolder { get; private set; }


        [Space]

        [Header("Death")]
        [SerializeField] private Behaviour[] disableOnDeath;
        [SerializeField] private GameObject[] disableGameObjectsOnDeath;
        [field: SyncVar] public bool isDead { get; private set; }

        //[SerializeField] private GameObject deathEffect;
        //[SerializeField] private GameObject spawnEffect;


        [Space]

        [Header("Audio")]
        private AudioSync audioSync;



        [Space]


        private bool[] wasEnabled;

        [SerializeField] private CharacterController characterController;

        [SerializeField] private LayerMask hitMask;

        private bool firstSetup = true;

        //[SerializeField] private GameObject compass;

        [SerializeField] private Transform killerPlayerInfoPrefab;
 
        [SerializeField] private AudioSource audioSource;



        private Scoreboard scoreboard;

        private Transform cam;

#region Actions
        public Action<string, int> OnGotHit;

        public Action<string, Team, string, int> OnDeath;
        public Action OnRespawn;

        public Action<string, string> OnKill;
#endregion


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

            MiniMapCameraMove _miniMapCameraMove = _miniMapCamera.GetComponent<MiniMapCameraMove>();
            _miniMapCameraMove.Setup(gameObject.transform);

            //GameObject _compass = GameObject.Instantiate(compass, CanvasInstance.instance.canvas.transform);
            //_compass.GetComponent<Compass>().reference = gameObject.transform.GetChild(0).GetChild(0);

        }

        [Command]
        private void InitializePlayerInfo(string name, Team newTeam)
        {
            team = newTeam;
            nickname = name;
        }

        private void Start()
        {
            audioSync = GetComponent<AudioSync>();
            if (isLocalPlayer)
            {
                gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
                gameObject.tag = "LocalPlayer";

                InitializePlayerInfo(PlayerInfoTransfer.instance.nickname, PlayerInfoTransfer.instance.team);

                Transform hitIndicator = Instantiate(hitIndicatorPrefab, CanvasInstance.instance.canvas.transform);
                hitIndicator.GetComponent<HitIndicatorTrigger>().Setup(this, orientation);

                SetupCameraHolder();
                SetupMiniMap();

                health.onDeath += Die;

                Instantiate(healthBarPrefab, CanvasInstance.instance.canvas.transform);

                GameObject menu = Instantiate(menuPrefab, CanvasInstance.instance.canvas.transform);
                menu.GetComponent<Menu>().look = cameraHolder.GetComponent<Look>();
                CanvasInstance.instance.canvas.transform.GetChild(0).gameObject.SetActive(true);

                Instantiate(killerPlayerInfoPrefab, CanvasInstance.instance.canvas.transform).GetComponent<KillerPlayerInfo>().Setup(this);
            }

            scoreboard = CanvasInstance.instance.scoreBoard.GetComponent<Scoreboard>();
        }

        public void SetTeam(Team oldTeam, Team newTeam)
        {
            GameObject playerRow = Instantiate(playerMark);
            playerRow.GetComponent<PlayerMark>().Setup(newTeam, isLocalPlayer, transform, gameObject.transform.GetChild(0).GetChild(0));

            if (isLocalPlayer)
            {
                Transform _spawnPoint = NetworkManagerFF.GetRespawnPosition(team);
                transform.position = _spawnPoint.position;
                transform.rotation = _spawnPoint.rotation;

                return;
            }

            GameObject localPlayerInstance = GameObject.FindGameObjectWithTag("LocalPlayer");
            Team localPlayerTeam = Team.Null;

            if (localPlayerInstance != null)
                localPlayerTeam = localPlayerInstance.GetComponent<GamePlayer>().team;

            else
                localPlayerTeam = PlayerInfoTransfer.instance.team;

             
            string playerTag = "";

            if (newTeam == Team.Blue && localPlayerTeam == Team.Blue)
                playerTag = "FriendlyPlayer";

            else if (newTeam == Team.Red && localPlayerTeam == Team.Red)
                playerTag = "FriendlyPlayer";

            else
                playerTag = "EnemyPlayer";

            gameObject.tag = playerTag;
        }

        private void SetNickname(string oldName, string newName)
        {
            if (isLocalPlayer)
                return;

            nameGO.SetActive(true);
            TMP_Text text = nameGO.GetComponentInChildren<TMP_Text>();

            //StartCoroutine(WaitForSyncTeam(text));
            text.color = TeamToColor.GetTeamColor(team);
            text.text = newName;
        }


        #region Weapon


        
        public IEnumerator Shoot(Ray[] rays, int damage, float shootRange, string playerID, float timeBetweenShots)
        {
            weaponKeyCodes.currentWeapon.canShoot = false;

            if(rays.Length<1 || rays.Length!=weaponKeyCodes.currentWeapon.numberOfBulletsPerShot)
                Debug.LogError("The number of patterns must be equal to the number of bullets per shot");
            for (int i = 0; i < rays.Length; i++)
            {
                
                if(weaponKeyCodes.currentWeapon.timeBetweenSpawnBullets != 0 || i == 0)
                    audioSync.PlaySound(0);
                if (!weaponKeyCodes.currentWeapon.useOneAmmoPerShot)
                {
                    weaponKeyCodes.currentWeapon.animator.StopPlayback();
                    weaponKeyCodes.currentWeapon.animator.Play(weaponKeyCodes.currentWeapon.shootAnimationName);
                    weaponKeyCodes.currentWeapon.weaponAmmo.Ammo--;
                    weaponKeyCodes.currentWeapon.weaponAmmo.UpdateAmmoInScreen();
                }
                bool isHitted = Physics.Raycast(rays[i], out RaycastHit hit, shootRange, hitMask);


                if (isHitted && !GameManager.instance.gameEnded)
                {
                    if (hit.transform.tag != "FriendlyPlayer")
                    {
                        Health hitHealth = hit.transform.GetComponent<Health>();
                        if (hitHealth)
                        {
                            StartCoroutine(ActivateForSeconds(CanvasInstance.instance.hitMarker, 0.15f));
                            CmdPlayerHit(hit.transform.GetComponent<NetworkIdentity>().netId.ToString(), damage, playerID);
                        }
                    }
                }

                CmdSpawnTrail(isHitted, rays[i].origin, rays[i].direction, hit.point, shootRange);
                if(i+1<rays.Length)
                    yield return new WaitForSeconds(timeBetweenShots);
                else if(rays.Length>1&&timeBetweenShots>0)
                    yield return new WaitForSeconds(0.07f);

            }
            weaponKeyCodes.currentWeapon.canShoot = true;
        }

        [Client]
        public void Punch(Ray ray, int damageToPlayer, int damageToOre, float punchDistance, float punchRadius, LayerMask hitLM, string playerID)
        {
            if (GameManager.instance.gameEnded)
                return;

            bool isHitted = Physics.SphereCast(ray, punchRadius, out RaycastHit hit, punchDistance, hitLM);
            if (isHitted)
            {
                if(hit.transform.tag == "FriendlyPlayer")
                    return;
                Health hitHealth = hit.transform.GetComponent<Health>();
                if (hitHealth)
                {
                    StartCoroutine(ActivateForSeconds(CanvasInstance.instance.hitMarker, 0.5f));
                    CmdPlayerHit(hit.transform.GetComponent<NetworkIdentity>().netId.ToString(), damageToPlayer, playerID);
                }
                else if (hit.transform.tag == "Ore")
                {
                    StartCoroutine(ActivateForSeconds(CanvasInstance.instance.hitMarker, 0.5f));

                    Ore _ore = hit.transform.GetComponent<Ore>();
                    CmdOreHit(damageToOre, GetNetID(), _ore);
                }
            }
        }

        [Command]
        private void CmdOreHit(int damageToOre, string netID, Ore ore)
        {
            ore.RpcCheckHP(damageToOre, netID);
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
        private void CmdPlayerHit(string _playerID, int _damage, string _sourceID)
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

            GamePlayer sourcePlayer = GameManager.GetPlayer(_sourceID);

            if (sourcePlayer != null)
            {
                OnDeath?.Invoke(_sourceID, sourcePlayer.team, sourcePlayer.nickname, (int)sourcePlayer.gameObject.GetComponent<Health>().currentHealth);
                //sourcePlayer.CmdAddKill();

                print($"die {_sourceID} {GetNetID()}");


                CmdPlayerKilled(GetNetID(), nickname, sourcePlayer.netIdentity);

                CmdDie(_sourceID);
            }

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
        private void CmdPlayerKilled(string killedNetID, string killedNickname, NetworkIdentity connToClient)
        {
            TargetPlayerKilled(connToClient.connectionToClient, killedNetID, killedNickname);
        }

        [TargetRpc]
        private void TargetPlayerKilled(NetworkConnection conn, string killedNetID, string killedNickname)
        {
            //print("on kill target RPC");
            NetworkClient.localPlayer.GetComponent<GamePlayer>().PlayerKilled(killedNetID, killedNickname);
        }

        [Client]
        private void PlayerKilled(string killedNetID, string killedNickname)
        {
            //print($"trpc {killedNetID} {GetNetID()}, {killedNickname} {nickname}");

            if (killedNetID != GetNetID())
            {
                UpdateKillsCount(1);
                OnKill?.Invoke(killedNetID, nickname);
            }
            else
            {
                UpdateKillsCount(-1);
                OnKill?.Invoke(killedNetID, nickname);
            }
        }

        [Command]
        private void UpdateKillsCount(int kill)
        {
            kills += kill;

            scoreboard.AddScore(team, kill, nickname);
        }



        [Command]
        private void CmdDie(string _sourceID)
        {
            //GamePlayer player = GameManager.GetPlayer(_sourceID);
            //player.kills++;
            //scoreboard.AddScore(player.team, 1, player.nickname);

            deaths++;

            RpcDie(_sourceID);
        }

        [ClientRpc]
        private void RpcDie(string _sourceID)
        {
            GamePlayer player = GameManager.GetPlayer(_sourceID);

            GameManager.instance.OnPlayerKilledCallback?.Invoke(nickname, team, player.nickname, player.team);
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

            if (!isLocalPlayer)
                nameGO.SetActive(false);

        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

            Transform _spawnPoint = NetworkManagerFF.GetRespawnPosition(team);
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

            health.Reset();

            //Enable the components
            for (int i = 0; i < disableOnDeath.Length; i++)
                disableOnDeath[i].enabled = wasEnabled[i];
            GetComponent<CharacterController>().enabled = true;
            //Enable the gameobjects
            for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
                disableGameObjectsOnDeath[i].SetActive(true);

            if (!isLocalPlayer)
                nameGO.SetActive(true);
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


        public string GetNetID() => netId.ToString();
    }
}