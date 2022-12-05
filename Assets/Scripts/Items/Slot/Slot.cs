using ItemSystem;

public class Slot
{
    public Item item;
    private bool isFiltering;
    public ItemType filteringType;

    public Item PutItem(Item item) 
        => this.item.PutItem(item);

    public Slot()
    {
        item = new Item();
        isFiltering = false;
        filteringType = ItemType.Nothing;
    }

    public Slot(Item item, bool isFiltering = false, ItemType filteringType = ItemType.Nothing)
    {
        this.item = item;
        this.isFiltering = isFiltering;
        this.filteringType = filteringType;
    }

    public Slot(int itemCount, ItemType itemType, bool isFiltering = false, ItemType filteringType = ItemType.Nothing)
    {
        this.item = new Item(itemCount, itemType);
        this.isFiltering = isFiltering;
        this.filteringType = filteringType;
    }
}