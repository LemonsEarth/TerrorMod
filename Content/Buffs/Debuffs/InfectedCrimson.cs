using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Core.Globals.NPCs.Corruption;
using TerrorMod.Core.Globals.NPCs.Crimson;
using TerrorMod.Core.Players;

namespace TerrorMod.Content.Buffs.Debuffs
{
    public class InfectedCrimson : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TerrorPlayer>().infected = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<InfectedCrimsonNPC>().infectedCrimson = true;
        }
    }
}
