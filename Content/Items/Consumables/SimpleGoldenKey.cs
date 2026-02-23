namespace TerrorMod.Content.Items.Consumables;

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
        recipe.AddIngredient(ItemID.GoldBar, 8);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();

        Recipe recipe2 = CreateRecipe();
        recipe2.AddIngredient(ItemID.PlatinumBar, 8);
        recipe2.AddTile(TileID.Anvils);
        recipe2.Register();
    }
}
