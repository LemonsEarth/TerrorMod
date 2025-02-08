using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Core.Globals.NPCs.Corruption;
using TerrorMod.Core.Players;

namespace TerrorMod.Content.Buffs.Debuffs
{
    public class FearDebuff : ModBuff
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
                Vector2 mousePos = Main.MouseWorld;
                Vector2 mouseToPlayer = mousePos.DirectionTo(player.Center);
                player.velocity += mouseToPlayer * 0.2f;
                player.controlDown = true;
            }
        }
    }
}
