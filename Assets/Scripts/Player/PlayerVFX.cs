using FiniteMovementStateMachine;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerVFX : NetworkBehaviour
{
    [SerializeField] private GamePlayer player;

    [Space(20)]

    [SerializeField] private Transform deathFX;
    [SerializeField] private Transform spawnFX;

    [Space(5)]

    [SerializeField] private Transform hitFX;

    [Space(5)]

    [SerializeField] private Transform jumpFX;
    [SerializeField] private Transform jumpFXPos;

    [Space(5)]

    [SerializeField] private Transform redirectFX;
    [SerializeField] private Transform redirectFXPos1;
    [SerializeField] private Transform redirectFXPos2;

    [Space]

    [SerializeField] private Transform trail;
    [SerializeField] private Transform muzzleFlash;
    public Transform muzzlePosition;

    [Space]

    [SerializeField] private GameObject damageVingette;
    [SerializeField] private Image damageVingetteImage;

    private void Start()
    {
        GetComponent<MovementMachine>().midAir.OnJump += SpawnJumpFX;

        damageVingette = CanvasInstance.instance.damageVingette;
        damageVingetteImage = damageVingette.GetComponent<Image>();
    }

    public void RedirectFX(Vector2 inputVector)
    {
        //bool isBlue = team == Team.Blue; // why not comment

        CmdSpawnRedirect(new Vector3(inputVector.x, 0, inputVector.y), player.team == Team.Blue);
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

        Transform effectInstance = Instantiate(redirectFX, redirectFXPos1.position, Quaternion.identity, transform);
        effectInstance.LookAt(redirectFXPos1.position + orientedVector);

        effectInstance.gameObject.GetComponent<VisualEffect>().SetBool("BlueTeam", isBlue);
        Destroy(effectInstance.gameObject, 0.13f);


        effectInstance = Instantiate(redirectFX, redirectFXPos2.position, Quaternion.identity, transform);
        effectInstance.LookAt(redirectFXPos2.position + orientedVector);

        effectInstance.gameObject.GetComponent<VisualEffect>().SetBool("BlueTeam", isBlue);
        Destroy(effectInstance.gameObject, 0.13f);
    }

    public void SpawnJumpFX()
    {
        CmdSpawnJumpFX();
    }
    [Command]
    public void CmdSpawnJumpFX()
    {
        RpcSpawnJumpFX();
    }
    [ClientRpc]
    public void RpcSpawnJumpFX()
    {
        if (isLocalPlayer)
            return;

        Transform _jumpEffect = Instantiate(jumpFX, jumpFXPos.position, Quaternion.identity, transform);
        Destroy(_jumpEffect.gameObject, .2f);
    }


    public void SpawnReSpawnFX()
    {

        CmdSpawnReSpawnFX(player.team == Team.Blue);
    }

    [Command]
    private void CmdSpawnReSpawnFX(bool isBlue)
    {
        RpcSpawnReSpawnFX(isBlue);
    }

    [ClientRpc]
    private void RpcSpawnReSpawnFX(bool isBlue)
    {
        Transform _spawnedEffect = Instantiate(spawnFX, transform.position, new Quaternion(0, transform.forward.y, 0, 1)).transform;

        _spawnedEffect.gameObject.GetComponent<VisualEffect>().SetBool("BlueTeam", isBlue);
        Destroy(_spawnedEffect.gameObject, 3);

    }
    public void SpawnDeathFX()
    {

        CmdSpawnDeathFX(player.team == Team.Blue);
    }

    [Command]
    private void CmdSpawnDeathFX(bool isBlue)
    {
        RpcSpawnDeathFX(isBlue);
    }

    [ClientRpc]
    private void RpcSpawnDeathFX(bool isBlue)
    {

        Transform _spawnedEffect = Instantiate(deathFX, transform.position, Quaternion.identity).transform;
        _spawnedEffect.LookAt(transform.forward);

        Destroy(_spawnedEffect.gameObject, 3);

    }

    public void SpawnHitFX(Vector3 position, Vector3 normal)
    {
        CmdSpawnHitFX(position, normal);
    }

    [Command]
    private void CmdSpawnHitFX(Vector3 position, Vector3 normal)
    {
        RpcSpawnHitFX(position, normal);
    }

    [ClientRpc]
    private void RpcSpawnHitFX(Vector3 position, Vector3 normal)
    {
        Transform _spawnedEffect = Instantiate(hitFX, position, Quaternion.identity);

        Destroy(_spawnedEffect.gameObject, 0.5f);
    }

    [Command]
    public void CmdSpawnTrail(bool isHitted, Vector3 origin, Vector3 direction, Vector3 point, float shootRange)
    {
        if ((origin - point).magnitude < 5f)
            return;

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
    public void CmdSpawnMuzzleFlash()
    {
        RpcSpawnMuzzleFlash();
    }

    [ClientRpc]
    private void RpcSpawnMuzzleFlash()
    {
        
        if (!muzzlePosition)
        {
            muzzlePosition = player.weaponKeyCodes.weaponHolder.GetChild(player.weaponKeyCodes.currentWeaponChildIndex).GetComponent<Weapon>().muzzlePosition;
        }
        Transform _muzzleFalsh = Instantiate(muzzleFlash, muzzlePosition.position, Quaternion.LookRotation(muzzlePosition.forward), muzzlePosition);
        if (isLocalPlayer)
            _muzzleFalsh.gameObject.layer = LayerMask.NameToLayer("Weapon");

        Destroy(_muzzleFalsh.gameObject, 0.2f);
    }


    [Client]
    private void PlayerDamagedVingette()
    {

    }
}
