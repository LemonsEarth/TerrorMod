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

namespace TerrorMod.Core.Systems
{
    public class EventSystem : ModSystem
    {
        public static bool hellbreachActive { get; private set; } = false;
        public static bool finishedHellbreach { get; private set; } = false;

        public override void PostUpdateWorld()
        {
            if (HellbreachStartCheck())
            {
                hellbreachActive = true;
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.TerrorMod.Messages.Hellbreach.StartMessage"), Color.OrangeRed);
            }

            if (hellbreachActive && Utils.GetDayTimeAs24FloatStartingFromMidnight() > 19.50f)
            {
                hellbreachActive = false;
                finishedHellbreach = true;
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.TerrorMod.Messages.Hellbreach.EndMessage"), Color.OrangeRed);
            }
        }

        bool HellbreachStartCheck()
        {
            int chanceDenominator = finishedHellbreach ? 1000 : 200;
            if (Utils.GetDayTimeAs24FloatStartingFromMidnight() > 7.50f && Utils.GetDayTimeAs24FloatStartingFromMidnight() < 8f && DayCountSystem.dayCount > 2 && !hellbreachActive)
            {
                if (Main.rand.NextBool(chanceDenominator)) return true;
            }
            return false;
        }

        public override void ClearWorld()
        {
            hellbreachActive = false;
            finishedHellbreach = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["hellbreachActive"] = hellbreachActive;
            tag["finishedHellbreach"] = finishedHellbreach;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            hellbreachActive = tag.GetBool("hellbreachActive");
            finishedHellbreach = tag.GetBool("finishedHellbreach");
        }
    }
}
