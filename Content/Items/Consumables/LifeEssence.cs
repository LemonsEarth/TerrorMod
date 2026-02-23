using TerrorMod.Core.Players;
using Terraria.DataStructures;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Items.Consumables;

public class LifeEssence : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 100;
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
        ItemID.Sets.AnimatesAsSoul[Type] = true;
        ItemID.Sets.ItemNoGravity[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 48;
        Item.maxStack = 20;
        Item.rare = ItemRarityID.Green;
        Item.useAnimation = 10;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.consumable = true;
        Item.UseSound = SoundID.Item4;
    }

    public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
    {
        itemGroup = ContentSamples.CreativeHelper.ItemGroup.BuffPotion;
    }

    public override bool CanUseItem(Player player)
    {
        return player.GetModPlayer<TerrorPlayer>().curseLevel > 0;
    }

    public override bool? UseItem(Player player)
    {
        LemonUtils.DustCircle(player.Center, 16, 10, DustID.GemDiamond, 1.2f);
        if (player.whoAmI == Main.myPlayer)
        {
            player.GetModPlayer<TerrorPlayer>().curseLevel--;
        }
        return true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(5);
        recipe.AddRecipeGroup("TerrorMod:AnyBossTrophy", 1);
        recipe.Register();
    }
}
