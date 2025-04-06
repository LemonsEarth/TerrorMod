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
using Terraria.WorldBuilding;
using TerrorMod.Common.Utils;

namespace TerrorMod.Core.Systems
{
    public class SkullSystem : ModSystem
    {
        public static bool blindSkullActive = false;
        public static bool vagrantSkullActive = false;
        public static bool savageSkullActive = false;
        public static bool toughLuckSkullActive = false;
        public static bool briarSkullActive = false;
        public static bool gluttonySkullActive = false;
        public static bool greedSkullActive = false;

        public override void ClearWorld()
        {
            blindSkullActive = false;
            vagrantSkullActive = false;
            savageSkullActive = false;
            toughLuckSkullActive = false;
            briarSkullActive = false;
            gluttonySkullActive = false;
            greedSkullActive = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["blindSkullActive"] = blindSkullActive;
            tag["vagrantSkullActive"] = vagrantSkullActive;
            tag["savageSkullActive"] = savageSkullActive;
            tag["toughLuckSkullActive"] = toughLuckSkullActive;
            tag["briarSkullActive"] = briarSkullActive;
            tag["gluttonySkullActive"] = gluttonySkullActive;
            tag["greedSkullActive"] = greedSkullActive;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            blindSkullActive = tag.GetBool("blindSkullActive");
            vagrantSkullActive = tag.GetBool("vagrantSkullActive");
            savageSkullActive = tag.GetBool("savageSkullActive");
            toughLuckSkullActive = tag.GetBool("toughLuckSkullActive");
            briarSkullActive = tag.GetBool("briarSkullActive");
            gluttonySkullActive = tag.GetBool("gluttonySkullActive");
            greedSkullActive = tag.GetBool("greedSkullActive");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(blindSkullActive, vagrantSkullActive, savageSkullActive, toughLuckSkullActive, briarSkullActive, gluttonySkullActive, greedSkullActive);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out blindSkullActive, out vagrantSkullActive, out savageSkullActive, out toughLuckSkullActive, out briarSkullActive, out gluttonySkullActive, out greedSkullActive);
        }
    }
}
