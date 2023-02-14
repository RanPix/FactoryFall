using Mirror;
using Player;
using UnityEngine;

public class Ore : NetworkBehaviour
{
    [field: SerializeField] public Team team { get; private set; }
    [SerializeField] private GameObject view;

    [Space]
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [field: Min(0), SerializeField] public float currentHealth { get; private set; }


    [Space]
    [Header("Respawn")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject gatherEffect;


    private void Awake()
    {
        currentHealth = maxHealth;

        GameManager.instance.OnMatchSettingsSettedup += Setup;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnMatchSettingsSettedup -= Setup;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (PlayerInfoTransfer.instance.team != team)
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyOre");
        }
    }


    private void Setup(bool teamsMatch)
    {
        if (GameManager.instance.matchSettings.gm != Gamemode.BTR)
            gameObject.SetActive(false);
    }


    [ClientRpc]
    public void RpcCheckHP(float damage, string netID)
    {
        currentHealth -= damage;
        //print("Current health - " + currentHealth);
        //print("");

        if (currentHealth < 1)
        {
            if (GameManager.GetPlayer(netID).gameObject == NetworkClient.localPlayer.gameObject)
            {
                OreInventoryItem _item = CanvasInstance.instance.oreInventory.GetComponent<OreInventory>().item;

                if (_item.currentCount < _item.maxCount)
                    CanvasInstance.instance.oreInventory.GetComponent<OreInventory>().item.currentCount++;
            }
            currentHealth = maxHealth;
        }
    }

    [Command]
    private void CmdSpawnHitEffect()
        => RpcSpawnHitEffect();

    [ClientRpc]
    private void RpcSpawnHitEffect()
    {
        Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity), 1f);
    }


    [Command]
    private void CmdSpawnGatherEffect()
        => RpcSpawnGatherEffect();

    [ClientRpc]
    private void RpcSpawnGatherEffect()
    {
        Destroy(Instantiate(gatherEffect, transform.position, Quaternion.identity), 1f);
    }
}