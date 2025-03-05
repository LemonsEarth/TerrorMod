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
    public class MythrilArmor : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.MythrilHat
                || entity.type == ItemID.MythrilHelmet
                || entity.type == ItemID.MythrilHood
                || entity.type == ItemID.MythrilChainmail
                || entity.type == ItemID.MythrilGreaves;
        }

        public override void UpdateEquip(Item item, Player player)
        {
            player.endurance += 5f / 100f;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            var line = new TooltipLine(Mod, "Terror:MythrilDR", "Increases damage reduction by 5%");
            tooltips.Add(line);
        }
    }
}
