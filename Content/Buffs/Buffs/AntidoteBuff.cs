using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Core.Globals.NPCs.Corruption;
using TerrorMod.Core.Players;

namespace TerrorMod.Content.Buffs.Buffs
{
    public class AntidoteBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[ModContent.BuffType<InfectedCorrupt>()] = true;
            player.buffImmune[ModContent.BuffType<InfectedCrimson>()] = true;
        }
    }
}
