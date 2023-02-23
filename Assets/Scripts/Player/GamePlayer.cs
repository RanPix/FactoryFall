using FiniteMovementStateMachine;
using GameBase;
using Mirror;
using System;
using System.Collections;
using TMPro;
using UI.Indicators;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    [RequireComponent(typeof(PlayerSetup))]
    public class GamePlayer : NetworkBehaviour
    {
#region SyncVar
        [field: SyncVar (hook = nameof(SetTeam))] public Team team { get; private set; } = Team.Null;
        [field: SyncVar (hook = nameof(SetNickname))] public string nickname { get; private set; }

        [field: SyncVar (hook = nameof(UpdateKillsCount))] public int kills { get; private set; }
        [field: SyncVar] public int deaths { get; private set; }
        [field: SyncVar (hook = nameof(UpdateScoreCount))] public int score { get; private set; }
#endregion


        [Header("Health")]
        [SerializeField] private Health health;
        [SerializeField] private GameObject healthBarPrefab;


        [Space]

        [Header("Weapon")]
        [SerializeField] private GameObject ammoTextPrefab;
        [SerializeField] private Transform trail;
        [SerializeField] private Transform hitIndicatorPrefab;
        [field: SerializeField] public WeaponKeyCodes weaponKeyCodes { get; private set; }

        public Transform muzzlePosition;



        [Space]

        [Header("UI")]
        [SerializeField] private InventoryUI inventory;
        [SerializeField] private GameObject menuPrefab;


        [Header("Player")]
        [SerializeField] private GameObject nameGO;

        [SerializeField] private GameObject playerModel;
        [SerializeField] private MeshRenderer playerMesh;

        [SerializeField] private Material bluePowerMat;
        [SerializeField] private Material blueBaseMat;
        [SerializeField] private Material redPowerMat;
        [SerializeField] private Material redBaseMat;

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

        [Header("Effects")]

        //[SerializeField] private GameObject deathEffect;
        //[SerializeField] private GameObject spawnEffect;

        [SerializeField] private Transform redirectEffect;
        [SerializeField] private Transform redirectEffectPos1;
        [SerializeField] private Transform redirectEffectPos2;

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
        private OreInventoryItem oreInventory;

        private Transform cam;

        private PlayerControls playerControls;

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
            InitializePlayerInfo(PlayerInfoTransfer.instance.nickname, PlayerInfoTransfer.instance.team);
            if (isLocalPlayer)
            {
                playerModel.SetActive(false);
                nameGO.SetActive(false);

                gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
                gameObject.tag = "LocalPlayer";

                Transform hitIndicator = Instantiate(hitIndicatorPrefab, CanvasInstance.instance.canvas.transform);
                hitIndicator.GetComponent<HitIndicatorTrigger>().Setup(this, orientation);

                SetupCameraHolder();
                SetupMiniMap();

                health.onDeath += Die;

                Instantiate(healthBarPrefab, CanvasInstance.instance.canvas.transform);

                GameObject menu = Instantiate(menuPrefab, CanvasInstance.instance.canvas.transform);
                menu.GetComponent<Menu>().look = cameraHolder.GetComponent<Look>();
                
                CanvasInstance.instance.canvas.transform.GetChild(0).gameObject.SetActive(true);

                gameObject.GetComponent<MovementMachine>().midAir.OnRedirect += RedirectFX;

                Instantiate(killerPlayerInfoPrefab, CanvasInstance.instance.canvas.transform).GetComponent<KillerPlayerInfo>().Setup(this);

                oreInventory = CanvasInstance.instance.oreInventory.GetComponent<OreInventoryItem>();
                OreGiveAwayArea.instance.OnAreaEnter += UpdateScore;
            }

            scoreboard = CanvasInstance.instance.scoreBoard.GetComponent<Scoreboard>();

            if (isLocalPlayer)
            {
                scoreboard.ChangeLocalPlayerScore(0);
            }
        }

        private void SetTeam(Team oldTeam, Team newTeam)
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

            Team localPlayerTeam = localPlayerInstance != null ?
                localPlayerInstance.GetComponent<GamePlayer>().team :
                PlayerInfoTransfer.instance.team;

            if (newTeam == Team.Blue)
            {
                playerMesh.materials[2].CopyPropertiesFromMaterial(blueBaseMat);
                playerMesh.materials[0].CopyPropertiesFromMaterial(bluePowerMat);
            }
            else if (newTeam == Team.Red)
            {
                playerMesh.materials[2].CopyPropertiesFromMaterial(redBaseMat);
                playerMesh.materials[0].CopyPropertiesFromMaterial(redPowerMat);
            }
             
            string playerTag;

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

            if(rays.Length < 1 || rays.Length != weaponKeyCodes.currentWeapon.weaponScriptableObject.numberOfBulletsPerShot)
                Debug.LogError("The number of patterns must be equal to the number of bullets per shot");


            for (int i = 0; i < rays.Length; i++)
            {
                if (weaponKeyCodes.currentWeapon.weaponAmmo.Ammo < 1)
                    break;

                if(weaponKeyCodes.currentWeapon.weaponScriptableObject.timeBetweenSpawnBullets != 0 || i == 0)
                    audioSync.PlaySound(ClipType.weapon,true, $"{weaponKeyCodes.currentWeapon.weaponName}_Shoot");
                

                if (!weaponKeyCodes.currentWeapon.weaponScriptableObject.useOneAmmoPerShot)
                {
                    weaponKeyCodes.currentWeapon.animator.StopPlayback();
                    weaponKeyCodes.currentWeapon.animator.Play(weaponKeyCodes.currentWeapon.shootAnimationName);
                    weaponKeyCodes.currentWeapon.weaponAmmo.Ammo--;
                    weaponKeyCodes.currentWeapon.weaponAmmo.UpdateAmmoInScreen();
                    weaponKeyCodes.currentWeapon.recoil.RecoilFire();
                }
                bool isHitted = Physics.Raycast(rays[i], out RaycastHit hit, shootRange, hitMask, QueryTriggerInteraction.Ignore);


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

                if(i + 1 < rays.Length)
                    yield return new WaitForSeconds(timeBetweenShots);

            }
            weaponKeyCodes.currentWeapon.canShoot = true;
        }

        [Client]
        public void Punch(Ray ray, int damageToPlayer, int damageToOre, float punchDistance, float punchRadius, LayerMask hitLM, string playerID)
        {
            if (GameManager.instance.gameEnded)
                return;

            bool isHitted = Physics.SphereCast(ray, punchRadius, out RaycastHit hit, punchDistance, hitLM, QueryTriggerInteraction.Ignore);
            if (isHitted)
            {
                if (hit.transform.tag != "FriendlyPlayer")
                {
                    Health hitHealth = hit.transform.GetComponent<Health>();

                    if (hitHealth)
                    {
                        StartCoroutine(ActivateForSeconds(CanvasInstance.instance.hitMarker, 0.5f));
                        CmdPlayerHit(hit.transform.GetComponent<NetworkIdentity>().netId.ToString(), damageToPlayer, playerID);
                        audioSync.PlaySound(ClipType.player, true, "Arm_HitInPlayer");
                        return;
                    }
                    else if (hit.transform.tag == "Ore")
                    {
                        StartCoroutine(ActivateForSeconds(CanvasInstance.instance.hitMarker, 0.5f));

                        Ore _ore = hit.transform.GetComponent<Ore>();
                        CmdOreHit(damageToOre, GetNetID(), _ore);
                        audioSync.PlaySound(ClipType.player, true, "Arm_HitInOre");
                        return;
                    }
                }


            }

            audioSync.PlaySound(ClipType.player, true, "Arm_Shot");
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
                CmdUpdateKillsCount(1);
                OnKill?.Invoke(killedNetID, nickname);
            }
            else
            {
                CmdUpdateKillsCount(-1);
                OnKill?.Invoke(killedNetID, nickname);
            }
        }



        [Command]
        private void CmdDie(string _sourceID)
        {
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
            {

                playerModel.SetActive(false);
                nameGO.SetActive(false);
            }

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
            {
                playerModel.SetActive(true);
                nameGO.SetActive(true);
            }
        }


        #endregion


        #region Score

        private void UpdateScore(int amount)
        {
            CmdUpdateScore(amount);
        }

        [Command]
        private void CmdUpdateScore(int amount)
        {
            score += amount;

            if (GameManager.instance.matchSettings.scoreBased)
                scoreboard.AddScore(team, amount, nickname);
        }

        [Command]
        private void CmdUpdateKillsCount(int kill)
        {
            kills += kill;

            if (!GameManager.instance.matchSettings.scoreBased)
                scoreboard.AddScore(team, kill, nickname);
        }

        [Client]
        private void UpdateKillsCount(int oldAmount, int newAmount)
        {
            if (!isLocalPlayer)
                return;

            if (GameManager.instance.matchSettings.scoreBased)
                return;

            scoreboard.ChangeLocalPlayerScore(newAmount);
        }

        [Client]
        private void UpdateScoreCount(int oldAmount, int newAmount)
        {
            if (!isLocalPlayer)
                return;

            if (!GameManager.instance.matchSettings.scoreBased)
                return;

            scoreboard.ChangeLocalPlayerScore(newAmount);
        }

        #endregion

        #region Effects


        private void RedirectFX(Vector2 inputVector)
        {
            //bool isBlue = team == Team.Blue; // why not comment

            CmdSpawnRedirect(new Vector3(inputVector.x, 0, inputVector.y), team == Team.Blue);
        }

        [Command]
        private void CmdSpawnRedirect(Vector3 orientedVector, bool isBlue)
        {
            RpcSpawnRedirect(orientedVector, isBlue);
        }

        [ClientRpc]
        private void RpcSpawnRedirect(Vector3 orientedVector, bool isBlue)
        {
            if (isLocalPlayer)
                return;

            Transform effectInstance = Instantiate(redirectEffect, redirectEffectPos1.position, Quaternion.identity, transform);
            effectInstance.LookAt(redirectEffectPos1.position + orientedVector);

            effectInstance.gameObject.GetComponent<VisualEffect>().SetBool("BlueTeam", isBlue);
            Destroy(effectInstance.gameObject, 0.13f);


            effectInstance = Instantiate(redirectEffect, redirectEffectPos2.position, Quaternion.identity, transform);
            effectInstance.LookAt(redirectEffectPos2.position + orientedVector);

            effectInstance.gameObject.GetComponent<VisualEffect>().SetBool("BlueTeam", isBlue);
            Destroy(effectInstance.gameObject, 0.13f);
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