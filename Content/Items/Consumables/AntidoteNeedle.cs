using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Buffs;
using Microsoft.Xna.Framework;

namespace TerrorMod.Content.Items.Consumables
{
    public class AntidoteNeedle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
            ItemID.Sets.DrinkParticleColors[Type] = new Color[2] {
                new Color(0, 255, 0),
                new Color(255, 0, 0)
            };
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Green;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.consumable = true;
            Item.UseSound = SoundID.NPCHit1;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BuffPotion;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(ModContent.BuffType<AntidoteBuff>(), 36000); // Adding the buff here instead of via Item.buffType prevents QoL mods from doing the infinite buff nonsense :D
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Lens);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.AddIngredient(ItemID.PlatinumBar, 3);
            recipe.AddTile(TileID.Bottles);
            recipe.AddCondition(Condition.InEvilBiome);
            recipe.Register();
        }
    }
}
