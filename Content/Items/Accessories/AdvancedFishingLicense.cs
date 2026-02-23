using TerrorMod.Core.Players;

namespace TerrorMod.Content.Items.Accessories;

public class AdvancedFishingLicense : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 34;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 0, 1, 0);
        Item.rare = ItemRarityID.Orange;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (player.GetModPlayer<FishingPlayer>().fishingPower < 3)
        {
            player.GetModPlayer<FishingPlayer>().fishingPower = 3;
        }
    }
}
