using ItemSystem;

public enum CraftableOn
{
    Furnace,
    Player,
    Assembler
}

public struct Recipe
{
    public string name { get; private set; }
    public Item[] neededItems { get; private set; }
    public Item[] craftResult { get; private set; }
    public float craftTime { get; private set; }
    public CraftableOn[] craftableOn { get; private set; }

    public Recipe(string name, Item[] neededItems, Item[] craftResult, float craftTime, CraftableOn[] craftableOn)
    {
        this.name = name;
        this.neededItems = neededItems;
        this.craftResult = craftResult;
        this.craftTime = craftTime;
        this.craftableOn = craftableOn;
    }

    public (Item[]/*rest*/, Item[]/*craft result*/) Craft(Item[] givenItems)
    {
        for (int i = 0; i < givenItems.Length; i++)
        {
            int a = 0;

            foreach (Item _item in neededItems)
                if (_item.itemType == givenItems[i].itemType)
                    a += _item.count;

            givenItems[i].count -= a;
        }

        return (givenItems, craftResult);
    }

    public bool CanCraft(Item[] givenItems, float craftingProgress)
    {
        if (craftingProgress < craftTime) return false;
        if (neededItems.Length < givenItems.Length)
            return false;
        
        bool result = true;

        foreach (Item givenItem in givenItems)
            result = result && !(neededItems > givenItem);

        return result;
    }

    public static bool operator ==(Recipe firstRecipe, Recipe secondRecipe)
    {
        bool isResultsEquals = firstRecipe.craftResult == secondRecipe.craftResult;
        bool isNeededItemsEquals = firstRecipe.neededItems == secondRecipe.neededItems;
        bool isCanCraftOnEquals = firstRecipe.craftableOn == secondRecipe.craftableOn;
        bool isCraftTimesOnEquals = firstRecipe.craftableOn == secondRecipe.craftableOn;
        return isResultsEquals && isNeededItemsEquals && isCanCraftOnEquals && isCraftTimesOnEquals;
    }

    public static bool operator !=(Recipe firstRecipe, Recipe secondRecipe) => !(firstRecipe == secondRecipe);
}
