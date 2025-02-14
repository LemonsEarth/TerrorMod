using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs;
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Corruption;

namespace TerrorMod.Core.Globals.Items.Armor
{
    public class TungstenArmor : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.TungstenHelmet;
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.TungstenHelmet && body.type == ItemID.TungstenChainmail && legs.type == ItemID.TungstenGreaves)
            {
                return "TungstenPenetration";
            }
            return string.Empty;
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "TungstenPenetration") player.AddBuff(ModContent.BuffType<TungstenPenetration>(), 2);
        }
    }
}
