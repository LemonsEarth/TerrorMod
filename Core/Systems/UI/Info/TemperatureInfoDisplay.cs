using Terraria.Localization;

namespace TerrorMod.Core.Systems.UI.Info;

public class TemperatureInfoDisplay : InfoDisplay
{
    public static LocalizedText CurrentTemperatureText { get; set; }

    public override void SetStaticDefaults()
    {
        CurrentTemperatureText = this.GetLocalization("CurrentTemperature");
    }

    public override string HoverTexture => Texture + "_Hover";

    public override bool Active()
    {
        return Main.LocalPlayer.TemperaturePlayer().ShowTemperature;
    }

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        var player = Main.LocalPlayer.TemperaturePlayer();
        Color color = Color.White;
        if (player.CurrentTemperature < player.SuperColdTolerance)
        {
            color = Color.DeepSkyBlue;
        }
        else if (player.CurrentTemperature < player.ColdTolerance)
        {
            color = Color.LightBlue;
        }
        else if (player.CurrentTemperature > player.SuperHeatTolerance)
        {
            color = Color.Red;
        }
        else if (player.CurrentTemperature > player.HeatTolerance)
        {
            color = Color.Orange;
        }

        displayColor = color;
        displayShadowColor = Color.Black;

        return CurrentTemperatureText.Format(MathF.Round(player.CurrentTemperature));
    }
}