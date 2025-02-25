using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace TerrorMod.Core.Configs
{
    public class TerrorClientConfigs : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static TerrorClientConfigs clientConfig;

        public override void OnLoaded()
        {
            clientConfig = this;
        }

        [Header("General")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool EnableRecipeRandomizer;
    }
}
