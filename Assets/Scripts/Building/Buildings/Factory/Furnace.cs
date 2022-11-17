using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Furnace : Block, IInteractable
{
    [Header("Furnace")]

    [SerializeField] private Item[] toSmelt;
    [SerializeField] private Item[] smelted;

    [SerializeField] private Item fuel;
    [SerializeField] private float smeltTime;
    private float smeltTimer;
    
    [Header("UI")]

    [SerializeField] private Transform furnaceUI;

    [SerializeField] private RecipeList recipeList;

    private void Start()
    {
        if (isLocalPlayer)
        {
            //furnaceUI = Instaniate(furnacceUI, GameObject.Find("Canvas").transform);
            //furnaceUI.SetActive(false);
        }
    }

    

    void Update()
    {
        if (!isServer)
            return;

        Smelt();
    }

    [Server]
    private void Smelt()
    {
        if (!CanSmelt())
            return;

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
    }

    private bool CanSmelt()
    {
        if (fuel < 1 || ItemArrLess(toSmelt, 1))
        {
            smeltTimer = 0f;
            return false;
        }

        smeltTimer += Time.deltaTime;

        if (smeltTimer < smeltTime)
            return false;
        smeltTimer = 0f;

        return true;//:skull:
    }

    public void Interact()
    {
        if (isLocalPlayer)
        {
            //furnaceUI.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        print($"dropped {toSmelt} queue items and {smelted} smelted items");
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
