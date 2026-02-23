using System.Collections.Generic;
using TerrorMod.Content.Items.Special;

namespace TerrorMod.Core.Players;

public class StartingInventoryPlayer : ModPlayer
{
    public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
    {
        itemsByMod["Terraria"].Clear();
    }

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        return new Item[] {
            new Item(ItemID.Wood, 16, 1),
            new Item(ItemType<LootToken>(), 1, 1)
        };
    }
}