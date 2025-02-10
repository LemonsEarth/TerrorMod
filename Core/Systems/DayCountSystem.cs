using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using TerrorMod.Common.Utils;

namespace TerrorMod.Core.Systems
{
    public class DayCountSystem : ModSystem
    {
        public static int dayCount = 0;

        public override void ClearWorld()
        {
            dayCount = 0;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            dayCount = tag.GetInt("dayCount");
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["dayCount"] = dayCount;
        }

        public override void PostUpdateWorld()
        {
            if ((int)Main.time == (int)Main.nightLength - 1)
            {
                dayCount++;
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(dayCount);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadInt32();
        }
    }
}
