using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using TerrorMod.Common.Utils;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.SceneEffects
{
    public class HellbreachScene : ModSceneEffect
    {
        public override int Music => MusicID.OtherworldlyUnderworld;
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;

        public override bool IsSceneEffectActive(Player player)
        {
            return EventSystem.hellbreachActive && player.ZoneOverworldHeight;
        }
    }
}
