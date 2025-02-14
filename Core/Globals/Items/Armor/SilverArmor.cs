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
    public class SilverArmor : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.SilverHelmet;
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.SilverHelmet && body.type == ItemID.SilverChainmail && legs.type == ItemID.SilverGreaves)
            {
                return "SilverProtection";
            }
            return string.Empty;
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "SilverProtection")
            {
                player.buffImmune[BuffID.Poisoned] = true;
                player.buffImmune[BuffID.Venom] = true;
            }
        }
    }
}
