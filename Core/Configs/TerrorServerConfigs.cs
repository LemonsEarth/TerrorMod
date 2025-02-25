using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace TerrorMod.Core.Configs
{
    public class TerrorServerConfigs : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static TerrorServerConfigs serverConfig;

        public override void OnLoaded()
        {
            serverConfig = this;
        }

        [Header("NPCs")]
        [DefaultValue(true)]
        public bool EnableBossChanges;
    }
}
