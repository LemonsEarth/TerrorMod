using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class BrainOfCthulhu : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override void SetDefaults(NPC entity)
        {
            entity.lifeMax = 1600;
            entity.defense = 16;
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.BrainofCthulhu;
        }

        public override void AI(NPC npc)
        {
            if (!npc.HasValidTarget) return;

            Main.NewText("ai0: " + npc.ai[0]);
            Main.NewText("ai1: " + npc.ai[1]);
            Main.NewText("ai2: " + npc.ai[2]);
            Main.NewText("ai3: " + npc.ai[3]);
            AITimer++;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<FearDebuff>(), 60);
        }
    }
}
