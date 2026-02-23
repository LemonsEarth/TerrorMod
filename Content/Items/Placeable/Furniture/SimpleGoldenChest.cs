namespace TerrorMod.Content.Items.Placeable.Furniture;

public class SimpleGoldenChest : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(TileType<Tiles.Furniture.SimpleGoldenChest>());
        // Item.placeStyle = 1; // Use this to place the chest in its locked style
        Item.width = 32;
        Item.height = 28;
        Item.value = 500;
    }

    // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.GoldChest);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}
