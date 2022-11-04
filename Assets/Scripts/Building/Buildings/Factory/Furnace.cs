using System.Collections.Generic;
using UnityEngine;

public class Furnace : Block, IInteractable
{
    [Header("Furnace")]

    [SerializeField] private int toSmelt;
    [SerializeField] private int smelted;

    [SerializeField] private int fuel;
    [SerializeField] private float smeltTime;
    private float smeltTimer;
     
    void Update()
    {
        Smelt();
    }

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
        print("smelt some cock");
    }

    private void OnDestroy()
    {
        //drop self inventory
    }
}
