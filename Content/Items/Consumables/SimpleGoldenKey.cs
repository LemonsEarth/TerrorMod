using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Buffs;
using Microsoft.Xna.Framework;

namespace TerrorMod.Content.Items.Consumables
{
    public class SimpleGoldenKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenKey);
            Item.width = 18;
            Item.height = 26;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.PlatinumBar, 4);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }
}
