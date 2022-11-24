using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ItemSystem;

public class Furnace : CraftingBlock
{
    [Header("Furnace")]
    [SerializeField] private float smeltTime;
    private float smeltTimer;
    
    [Header("UI")]

    [SerializeField] private Transform furnaceUIPrefab;
    private Transform furnaceUI;

    private void Start()
    {
        if (isLocalPlayer)
        {
            furnaceUI = Instantiate(furnaceUI, GameObject.Find("Canvas").transform);
            furnaceUI.gameObject.SetActive(false);
        }
    }
    
    

    void Update()
    {
        if (!isServer)
            return;

        //Smelt();
    }

    [Server]
    /*private void Smelt()
    {
        if (!CanSmelt())
            return;

        Recipe foundRecipe = recipeList.Recipes[0];
        foreach (Recipe recipe in recipeList.Recipes)
        {
            if (recipe.CanCraft(toSmelt, ))
            {
                foundRecipe = recipe;
                break;
            }
            else
                return;
        }

        (Item[], Item[]) SmeltResult = foundRecipe.Craft(toSmelt);

        Recipe foundRecipe;
        foreach (Recipe recipe in recipeList.Recipes)
        {
            if (recipe.CanCraft(toSmelt))
            {
                foundRecipe = recipe;
                break;
            }
            else
                return;
        }

        //(Item[], Item[]) SmeltResult = foundRecipe.Craft(toSmelt);

        //toSmelt = SmeltResult.Item1;
        //smelted = SmeltResult.Item2;
        
        fuel.count--;
    }*/

    private bool CanSmelt()
    {
        /*if (fuel < 1 || ItemArrLess(toSmelt, 1))
        {
            smeltTimer = 0f;
            return false;
        }

        smeltTimer += Time.deltaTime;

        if (smeltTimer < smeltTime)
            return false;
        smeltTimer = 0f;
        */
        return true;//:skull:
    }

    public void Interact(GameObject inventoryObject)
    {
        /*if (isLocalPlayer)
        {
            furnaceUI.gameObject.SetActive(true);
        }*/
    }

    private void OnDestroy()
    {
        //print($"dropped {toSmelt} queue items and {smelted} smelted items");
        //drop self inventory
    }

    private bool ItemArrLess(Item[] itemArray, int amount)
    {
        int a = 0;
        foreach (Item item in itemArray)
            a += item.count;

        return a > amount;
    }
}
