using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.NPCs.FrostLegion
{
    public class FrostLegionNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.SnowBalla
                || entity.type == NPCID.SnowmanGangsta
                || entity.type == NPCID.MisterStabby;
        }

        public override void SetDefaults(NPC entity)
        {
            if (entity.type == NPCID.SnowBalla)
            {
                entity.defense = 32;
                entity.lifeMax = 700;
            }

            else if (entity.type == NPCID.SnowmanGangsta)
            {
                entity.defense = 28;
                entity.lifeMax = 660;
            }

            else if (entity.type == NPCID.MisterStabby)
            {
                entity.defense = 46;
                entity.lifeMax = 600;
                entity.knockBackResist = 0f;
                entity.Opacity = 0.2f;
            }
        }

        public override void AI(NPC npc)
        {
            if (AITimer == 0)
            {
                LemonUtils.DustCircle(npc.Center, 8, 8, DustID.GemDiamond, Main.rand.NextFloat(1.5f, 2.5f));
            }

            if (npc.type == NPCID.SnowBalla)
            {
                if (AITimer % 180 == 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * i) * Main.rand.NextFloat(2, 8), ProjectileID.SnowBallHostile, npc.damage / 4, 1f);
                        }
                    }
                }
            }

            else if (npc.type == NPCID.SnowmanGangsta)
            {
                if (npc.ai[2] < 100) npc.ai[2] = 100;
            }

            else if (npc.type == NPCID.MisterStabby)
            {
                
            }
            AITimer++;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            
        }
    }
}
