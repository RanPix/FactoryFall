using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Furnace : Block, IInteractable
{
    [Header("Furnace")]

    [SyncVar][SerializeField] private int toSmelt;
    [SyncVar][SerializeField] private int smelted;

    [SyncVar][SerializeField] private int fuel;
    [SerializeField] private float smeltTime;
    private float smeltTimer;

    void Update()
    {
        if (!isServer)
            return;

        Smelt();
    }

    [Server]
    private void Smelt()
    {
        if (fuel < 1 || toSmelt < 1)
            return;

        smeltTimer += Time.deltaTime;

        if (smeltTimer < smeltTime)
            return;
        smeltTimer = 0f;

        smelted++;
        toSmelt--;

        fuel--;
    }

    public void Interact()
    {
        if (!isServer)
            return;

        fuel++;
        toSmelt++;
        print("smelt some integers");
    }

    private void OnDestroy()
    {
        print($"dropped {toSmelt} queue items and {smelted} smelted items");
        //drop self inventory
    }
}
