using Microsoft.Xna.Framework;
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
    public class LeadArmor : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.LeadHelmet;
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.LeadHelmet && body.type == ItemID.LeadChainmail && legs.type == ItemID.LeadGreaves)
            {
                return "LeadSkin";
            }
            return string.Empty;
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "LeadSkin") player.GetModPlayer<TerrorPlayer>().leadArmorSet = true;
        }
    }
}
