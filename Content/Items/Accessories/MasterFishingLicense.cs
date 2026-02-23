using TerrorMod.Core.Players;

namespace TerrorMod.Content.Items.Accessories;

public class MasterFishingLicense : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 34;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 0, 1, 0);
        Item.rare = ItemRarityID.Pink;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (player.GetModPlayer<FishingPlayer>().fishingPower < 4)
        {
            player.GetModPlayer<FishingPlayer>().fishingPower = 4;
        }
    }
}
