using TerrorMod.Core.Players;

namespace TerrorMod.Content.Items.Accessories;

public class IntermediateFishingLicense : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 34;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 0, 1, 0);
        Item.rare = ItemRarityID.Green;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (player.GetModPlayer<FishingPlayer>().fishingPower < 2)
        {
            player.GetModPlayer<FishingPlayer>().fishingPower = 2;
        }
    }
}
