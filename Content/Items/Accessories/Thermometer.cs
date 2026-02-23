using TerrorMod.Core.Players;

namespace TerrorMod.Content.Items.Accessories;

public class Thermometer : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 56;
        Item.height = 56;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 2);
        Item.rare = ItemRarityID.Green;
    }

    public override void UpdateInfoAccessory(Player player)
    {
        player.TemperaturePlayer().ShowTemperature = true;
    }
}
