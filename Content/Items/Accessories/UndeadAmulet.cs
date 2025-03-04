using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TerrorMod.Core.Players;

namespace TerrorMod.Content.Items.Accessories
{
    public class UndeadAmulet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 36;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 8);
            Item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TerrorPlayer>().undeadAmulet = true;
        }
    }
}
