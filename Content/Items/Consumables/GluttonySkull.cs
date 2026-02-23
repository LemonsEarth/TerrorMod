using TerrorMod.Core.Systems;
using Terraria.Localization;

namespace TerrorMod.Content.Items.Consumables;

public class GluttonySkull : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 30;
    }

    public override void SetDefaults()
    {
        Item.width = 64;
        Item.height = 86;
        Item.UseSound = SoundID.Item119;
        Item.useStyle = ItemUseStyleID.EatFood;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.consumable = true;
        Item.maxStack = 1;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddCondition(new Condition(LocalizedText.Empty, () => !SkullSystem.gluttonySkullActive));
        recipe.Register();
    }

    public override bool? UseItem(Player player)
    {
        SkullSystem.gluttonySkullActive = true;
        Item.stack--;
        return true;
    }
}

public class GluttonyPlayer : ModPlayer
{
    public override void UpdateBadLifeRegen()
    {
        if (SkullSystem.gluttonySkullActive)
        {
            Player.lifeRegen -= (int)(Player.manaSickReduction * 10) * 2;
        }
    }
}