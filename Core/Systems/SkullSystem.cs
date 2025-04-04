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
    public class SkullSystem : ModSystem
    {
        public static bool blindSkullActive = false;
        public static bool vagrantSkullActive = false;

        public override void ClearWorld()
        {
            blindSkullActive = false;
            vagrantSkullActive = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["blindSkullActive"] = blindSkullActive;
            tag["vagrantSkullActive"] = vagrantSkullActive;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            blindSkullActive = tag.GetBool("blindSkullActive");
            vagrantSkullActive = tag.GetBool("vagrantSkullActive");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(blindSkullActive, vagrantSkullActive);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out blindSkullActive, out vagrantSkullActive);
        }
    }
}
