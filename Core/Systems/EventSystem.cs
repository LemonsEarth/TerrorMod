using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static bool hellbreachActive = false;
        public static bool finishedHellbreach = false;
        public static List<Vector2> mechanicalCorePositions = new List<Vector2>(){Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero}; 

        public override void PostUpdateWorld()
        {
            if (HellbreachStartCheck())
            {
                hellbreachActive = true;
                if (Main.netMode != NetmodeID.MultiplayerClient) ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.TerrorMod.Messages.Hellbreach.StartMessage"), Color.OrangeRed);
            }

            if (hellbreachActive && Utils.GetDayTimeAs24FloatStartingFromMidnight() > 19.50f)
            {
                hellbreachActive = false;
                finishedHellbreach = true;
                if (Main.netMode != NetmodeID.MultiplayerClient) ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.TerrorMod.Messages.Hellbreach.EndMessage"), Color.OrangeRed);
            }

            if ((int)Main.time == 1 && !Main.dayTime && DayCountSystem.dayCount == 3)
            {
                Main.bloodMoon = true;
                if (Main.netMode != NetmodeID.MultiplayerClient) ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("The Bloody Moon rises..."), Color.Red);
            }
        }

        bool HellbreachStartCheck()
        {
            int chanceDenominator = !finishedHellbreach ? 3 : 20;
            if (Math.Floor(Main.time) == 1 && Main.dayTime && DayCountSystem.dayCount > 4 && !hellbreachActive)
            {
                if (Main.rand.NextBool(chanceDenominator)) return true;
            }
            return false;
        }

        public override void ClearWorld()
        {
            hellbreachActive = false;
            finishedHellbreach = false;
            for (int i = 0; i < mechanicalCorePositions.Count; i++)
            {
                mechanicalCorePositions[i] = Vector2.Zero;
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["hellbreachActive"] = hellbreachActive;
            tag["finishedHellbreach"] = finishedHellbreach;
            tag["mechanicalCorePositions"] = mechanicalCorePositions;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            hellbreachActive = tag.GetBool("hellbreachActive");
            finishedHellbreach = tag.GetBool("finishedHellbreach");
            mechanicalCorePositions = tag.GetList<Vector2>("mechanicalCorePositions").ToList<Vector2>();
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(hellbreachActive, finishedHellbreach);
            foreach (var pos in mechanicalCorePositions)
            {
                writer.WriteVector2(pos);
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out hellbreachActive, out finishedHellbreach);
            for (int i = 0; i < 4; i++)
            {
                mechanicalCorePositions[i] = reader.ReadVector2();
            }
        }
    }
}
