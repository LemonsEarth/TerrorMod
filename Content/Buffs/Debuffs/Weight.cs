using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Core.Globals.NPCs.Corruption;
using TerrorMod.Core.Players;

namespace TerrorMod.Content.Buffs.Debuffs
{
    public class Weight : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                player.velocity.Y += 2f;
            }
        }
    }
}
