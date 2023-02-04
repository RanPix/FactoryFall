using System.Collections;
using Mirror;
using UnityEngine;

public class Ore : NetworkBehaviour
{
    [SerializeField] private GameObject view;


    [Space]
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [field: Min(0), SerializeField] public float currentHealth { get; private set; }


    [Space]
    [Header("Respawn")]
    [SerializeField] private float timeToRespawn;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject spawnEffect;


    void Start()
    {
        currentHealth = maxHealth;
    }

    

    [ClientRpc]
    public void RpcCheckHP(float damage, string netID)
    {
        currentHealth -= damage;
        print("Current health - " + currentHealth);
        print("");

        if (currentHealth < 1)
        {
            if (GameManager.GetPlayer(netID).gameObject == NetworkClient.localPlayer.gameObject)
            {
                OreInventoryItem _item = CanvasInstance.instance.oreInventory.GetComponent<OreInventory>().item;

                if (_item.currentCount<_item.maxCount)
                    CanvasInstance.instance.oreInventory.GetComponent<OreInventory>().item.currentCount++;
            }
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        //GameObject _deathEffect = NetworkManager.Instantiate(deathEffect, new Vector3(0, 0, 0), Quaternion.identity, transform);
        //NetworkServer.Spawn(_deathEffect);
        view.SetActive(false);

        yield return new WaitForSeconds(timeToRespawn);

        currentHealth = maxHealth;
        view.SetActive(true);
        //GameObject _spawnEffect = NetworkManager.Instantiate(spawnEffect, new Vector3(0, 0, 0), Quaternion.identity, transform);
        //NetworkServer.Spawn(_spawnEffect);
    }

}