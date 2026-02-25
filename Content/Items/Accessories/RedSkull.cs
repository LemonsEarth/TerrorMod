using Terraria.Localization;
using TerrorMod.Content.Buffs.Debuffs.Movement;

namespace TerrorMod.Content.Items.Accessories;

public class RedSkull : ModItem
{
    static readonly float damageBoost = 25f;
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(damageBoost);

    public override void SetDefaults()
    {
        Item.width = 64;
        Item.height = 86;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 20);
        Item.rare = ItemRarityID.Red;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Generic) += damageBoost / 100f;
        player.buffImmune[BuffType<Weight>()] = true;
    }
}
