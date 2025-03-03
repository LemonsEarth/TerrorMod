using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class EaterOfWorlds : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.EaterofWorldsHead
                || entity.type == NPCID.EaterofWorldsBody
                || entity.type == NPCID.EaterofWorldsTail;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            if (npc.ai[0] % 4 == 0 && AITimer % (420 + npc.ai[0]) == 0) // ai0 is the segment number
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), player.Center - Vector2.UnitY * 900, Vector2.UnitY * 4, ProjectileID.CultistBossFireBallClone, npc.damage, 1f);
                }
            }

            if (AITimer % npc.ai[0] == 0 && (NPC.CountNPCS(NPCID.EaterofWorldsBody) + NPC.CountNPCS(NPCID.EaterofWorldsHead) + NPC.CountNPCS(NPCID.EaterofWorldsTail)) <= 45)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        Vector2 pos = player.Center + new Vector2(70 * i, 900);
                        Projectile.NewProjectile(npc.GetSource_FromAI(), pos, -Vector2.UnitY * 20, ModContent.ProjectileType<EyeFireClone>(), npc.damage / 2, 1f);
                    }
                }
            }

            AITimer++;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<FearDebuff>(), 60);
        }
    }
}
