using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class CraftingBlock : Block
{
    [SerializeField] CraftableOn type;
    [SerializeField] private RecipeList recipeList;

    public Recipe currentRecipe { get; private set; }

    public Slot[] inputSlots;
    public Slot[] outputSlots;

    bool canCraft = true;
    float craftingProgress = 0;

    private void Awake()
    {
        currentRecipe = recipeList.Recipes[0];
    }

    private void Update()
    {
        Crafting();
    }

    private void CanCraft()
    {

    }

    private void Crafting()
    {
        
        if (!canCraft)
            return;

    }

    public bool CanSetRecipe(Recipe recipe)
    {
        bool result = false;
        foreach (CraftableOn craftableOn in recipe.craftableOn)
        {
            result = result || craftableOn == type;
        }
        return result;
    }

    public void TrySetRecipe(Recipe recipe)
    {
        if (CanSetRecipe(recipe))
            SetRecipe(recipe);
    }

    protected void SetRecipe(Recipe recipe)
    {
        currentRecipe = recipe;
        
        foreach (Item item in recipe.neededItems)
        {
            
        }
        
    }
}
