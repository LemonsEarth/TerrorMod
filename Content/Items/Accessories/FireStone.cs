using TerrorMod.Content.Items.Special;
using TerrorMod.Content.Items.Tools;
using TerrorMod.Core.Players;

namespace TerrorMod.Content.Items.Accessories;

public class FireStone : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 0, 50, 0);
        Item.rare = ItemRarityID.Blue;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.TemperaturePlayer().TargetTemperature += 10;
        player.GetModPlayer<FireStonePlayer>().Active = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemType<TheRock>(), 1);
        recipe.AddIngredient(ItemID.LavaBucket, 10);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}

public class FireStonePlayer : ModPlayer
{
    public bool Active { get; set; } = false;

    public override void ResetEffects()
    {
        Active = false;
    }

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Active)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
