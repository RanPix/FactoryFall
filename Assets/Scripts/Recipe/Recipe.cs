public enum CraftableOn
{
    Furnace,
    Player,
    Assembler
}

public struct Recipe
{
    private Item[] neededItems;
    private Item[] craftResult;
    

    private CraftableOn[] canCraftOn;

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

    public bool CanCraft(Item[] givenItems)
    {
        if (neededItems.Length < givenItems.Length)
            return false;
        
        bool result = true;

        foreach (Item givenItem in givenItems)
            result = result && !(neededItems > givenItem);

        return result;
    }
}
