namespace TerrorMod.Content.Items.Accessories;

public class GreenSkull : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 64;
        Item.height = 86;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 20);
        Item.rare = ItemRarityID.Green;
        Item.lifeRegen = 6;
        Item.defense = 15;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.noKnockback = true;
    }
}
