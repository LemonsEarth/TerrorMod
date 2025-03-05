using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Corruption;
using TerrorMod.Core.Players;

namespace TerrorMod.Core.Globals.Items.Armor
{
    public class CobaltArmor : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.CobaltHat
                || entity.type == ItemID.CobaltHelmet
                || entity.type == ItemID.CobaltMask
                || entity.type == ItemID.CobaltBreastplate
                || entity.type == ItemID.CobaltLeggings;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string text = string.Empty;
            if (item.type == ItemID.CobaltHat
                || item.type == ItemID.CobaltHelmet
                || item.type == ItemID.CobaltMask)
            {
                text = "Chance to inflict Poisoned or Venom on hit";
            }
            else if (item.type == ItemID.CobaltBreastplate)
            {
                text = "Chance to inflict Shadowflame or Cursed Flames on hit";
            }
            else
            {
                text = "Chance to inflict Ichor or Oiled on hit";
            }
            var line = new TooltipLine(Mod, "Terror:CobaltDebuffs", text);
            tooltips.Add(line);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            if (item.type == ItemID.CobaltHat
                || item.type == ItemID.CobaltHelmet
                || item.type == ItemID.CobaltMask)
            {
                player.GetModPlayer<TerrorPlayer>().cobaltHead = true;
            }

            if (item.type == ItemID.CobaltBreastplate)
            {
                player.GetModPlayer<TerrorPlayer>().cobaltBody = true;
            }

            if (item.type == ItemID.CobaltLeggings)
            {
                player.GetModPlayer<TerrorPlayer>().cobaltLegs = true;
            }
        }
    }
}
