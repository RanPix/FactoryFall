using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class CraftingBlock : Block
{
    CraftableOn type;
    [SerializeField] private RecipeList recipeList;
    Recipe currentRecipe;
    
    [Header("Slots")]

    [Header("Electricity")]

    [SerializeField] bool haveElectricity;
    [SerializeField] bool needElectricity;

    [Header("Fuel")]

    [SerializeField] bool haveFuel;
    [SerializeField] bool needFuel;

    bool canCraft = true;
    Item[] outputItems;
    Item[] FuelItems;

    private void Awake()
    {
        currentRecipe = recipeList.Recipes[0];
    }

    private void Update()
    {
        Crafting();
    }

    private void Crafting()
    {
        BlockInventory inventory = gameObject.GetComponent<BlockInventory>();
        bool isCanCraftOn = false;
        foreach (CraftableOn craftableOn in currentRecipe.craftableOn)
        {
            if (type == craftableOn)
            {
                isCanCraftOn = true;
                break;
            }
        }

        if (isCanCraftOn && inventory != null && needFuel ? haveFuel : true && needElectricity ? haveElectricity : true && canCraft)
        {
            
        }
    }

    public void SetRecipe(Recipe recipe)
    {
        currentRecipe = recipe;
        foreach (Item item in recipe.neededItems)
        {

        }
        
    }
}
