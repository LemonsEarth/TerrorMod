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

namespace TerrorMod.Core.Globals.Items.Armor
{
    public class SnowArmor : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.EskimoHood;
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.EskimoHood && body.type == ItemID.EskimoCoat && legs.type == ItemID.EskimoPants)
            {
                return "Eskimo";
            }
            return string.Empty;
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "Eskimo") player.AddBuff(BuffID.Warmth, 2);
        }
    }
}
