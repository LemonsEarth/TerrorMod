using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Forest;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class Deerclops : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Deerclops;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            if (AITimer > 300 && AITimer < 600)
            {
                if (AITimer % 60 == 0)
                {
                    foreach (var ply in Main.ActivePlayers)
                    {
                        LemonUtils.DustCircle(player.Center, 8, 10, DustID.GemDiamond, 2f);
                    }
                }
            }

            if (npc.life <= npc.lifeMax / 2)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && AITimer % 300 == 0)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), player.Center - Vector2.UnitY * 300, Vector2.Zero, ModContent.ProjectileType<InfiniteTerrorHeadProj>(), npc.damage / 4, 1f, ai0: MathHelper.PiOver2, ai1: 60, ai2: 20);
                }
            }

            if (npc.life <= npc.lifeMax / 4)
            {
                if (npc.localAI[2] < 120 && npc.localAI[2] > 0) npc.localAI[2] = 120;
            }

            if (AITimer >= 600)
            {
                foreach (var ply in Main.ActivePlayers)
                {
                    ply.AddBuff(BuffID.Frozen, 60);
                }
                AITimer = 0;
            }
            AITimer++;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {

        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {

        }
    }
}
