namespace ProjectP.ItemSystem;

static class ItemCategories
{
    public const string ItemInfo = "ItemInfo";
    public const string ItemGraphics = "Graphics";
}

[GameResource("Item Definition", "item", "Describes basic item data things")]
public partial class Item : GameResource
{
    [Category(ItemCategories.ItemInfo)]
    public string Name { get; set; }
    
    [Category(ItemCategories.ItemInfo)]
    public string Description { get; set; }

    [Category(ItemCategories.ItemGraphics)]
    [ResourceType("vtex")]
    public string Icon { get; set; }

    [Category(ItemCategories.ItemGraphics)]
    [ResourceType("vmdl")]
    public string Model {get; set; }

    public Item() {

    }
}
