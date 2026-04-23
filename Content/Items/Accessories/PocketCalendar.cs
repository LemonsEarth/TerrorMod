using TerrorMod.Core.Players;

namespace TerrorMod.Content.Items.Accessories;

public class PocketCalendar : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 44;
        Item.height = 48;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 2);
        Item.rare = ItemRarityID.Green;
    }

    public override void UpdateInfoAccessory(Player player)
    {
        player.GetModPlayer<PocketCalendarPlayer>().ShowDaysPassed = true;
    }
}

public class PocketCalendarPlayer : ModPlayer
{
    public bool ShowDaysPassed { get; set; } = false;

    public override void ResetInfoAccessories()
    {
        ShowDaysPassed = false;
    }

    public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer)
    {
        if (otherPlayer.GetModPlayer<PocketCalendarPlayer>().ShowDaysPassed)
        {
            ShowDaysPassed = true;
        }
    }
}