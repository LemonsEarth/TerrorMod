using Terraria.Localization;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Content.Items.Accessories;

public class BlueSkull : ModItem
{
    static readonly float drBoost = 25f;
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(drBoost);

    public override void SetDefaults()
    {
        Item.width = 64;
        Item.height = 86;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 20);
        Item.rare = ItemRarityID.Blue;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.endurance += drBoost / 100f;
        player.buffImmune[BuffType<FearDebuff>()] = true;
    }
}
