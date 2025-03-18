using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Core.Globals.NPCs.Corruption;
using TerrorMod.Core.Players;

namespace TerrorMod.Content.Buffs.Debuffs
{
    public class HalloweenHorrorDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TerrorPlayer>().halloweenHorror = true;
        }
    }
}
