using System.Collections;
using Mirror;
using UnityEngine;

public class Ore : NetworkBehaviour
{
    [SerializeField] private GameObject view;

    [Space]
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [field:Min(0)][field: SerializeField][field: SyncVar(hook = nameof(CheckHP))] public float currentHealth { get; private set; }

    [Space]
    [Header("Respawn")]
    [SerializeField] private float timeToRespawn;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject spawnEffect;
    // Start is called before the first frame update 
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame 
    void Update()
    {

    }

    public void CmdGetDamage(float damage)
    {
        print("ore hit");
        RpcDSF(damage);
        currentHealth -= damage;
    }

    [ClientRpc]
    private void RpcDSF(float damage)
    {
        print($"damage - {damage}");
    }

    public void CheckHP(float oldHP, float newHP)
    {
        if (currentHealth < 1)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        /*GameObject _deathEffect = NetworkManager.Instantiate(deathEffect, new Vector3(0, 0, 0), Quaternion.identity, transform);
        NetworkServer.Spawn(_deathEffect);*/
        view.SetActive(false);

        yield return new WaitForSeconds(timeToRespawn);

        currentHealth = maxHealth;
        view.SetActive(true);
        /*GameObject _spawnEffect = NetworkManager.Instantiate(spawnEffect, new Vector3(0, 0, 0), Quaternion.identity, transform);
        NetworkServer.Spawn(_spawnEffect);*/
    }

}