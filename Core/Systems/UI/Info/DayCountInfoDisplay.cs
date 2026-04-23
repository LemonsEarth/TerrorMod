using Humanizer;
using Terraria.Localization;
using TerrorMod.Content.Items.Accessories;
using TerrorMod.Core.Systems.World;

namespace TerrorMod.Core.Systems.UI.Info;

public class DayCountInfoDisplay : InfoDisplay
{
    public static LocalizedText DaysPassedText { get; set; }

    public override void SetStaticDefaults()
    {
        DaysPassedText = this.GetLocalization("DaysPassed");
    }

    public override string HoverTexture => Texture + "_Hover";

    public override bool Active()
    {
        return Main.LocalPlayer.GetModPlayer<PocketCalendarPlayer>().ShowDaysPassed;
    }

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        var player = Main.LocalPlayer.GetModPlayer<PocketCalendarPlayer>();
        Color color = Color.White;

        displayColor = color;
        displayShadowColor = Color.Black;

        return DaysPassedText.Format(MathF.Round(DayCountSystem.DayCount), SeasonSystem.CurrentSeason.Humanize());
    }
}