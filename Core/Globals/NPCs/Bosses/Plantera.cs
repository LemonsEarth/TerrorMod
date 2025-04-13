using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Forest;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Configs;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class Plantera : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        int AttackTimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Plantera;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;

            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            // localai0 is phase

            if (npc.localAI[0] == 1)
            {
                if (AITimer % 120 == 0)
                {
                    npc.velocity *= 2.5f;
                    npc.netUpdate = true;
                }
                if (AITimer % 240 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 direction = Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * i);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, direction * 10, ProjectileID.ThornBall, npc.damage / 4, 1);
                        }
                    }
                }
            }
            else
            {
                if (npc.localAI[1] < 200) npc.localAI[1] = 200;
                if (AttackTimer > 600)
                {
                    AttackTimer = 0;
                }
                else if (AttackTimer > 300)
                {
                    if (AttackTimer % 15 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 pos = npc.Center + npc.Center.DirectionTo(player.Center) * (npc.width / 2);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.DirectionTo(player.Center) * 20, ModContent.ProjectileType<EyeFireButVenom>(), npc.damage / 5, 1f, ai1: 4, ai2: 3);
                        }
                    }
                }
                else
                {
                    if (AttackTimer % 60 == 0 && AttackTimer < 240 && AttackTimer > 0)
                    {
                        npc.velocity = npc.DirectionTo(player.Center) * 10;
                        npc.netUpdate = true;
                    }
                }

                if (AttackTimer == 270 || AttackTimer == 240)
                {
                    LemonUtils.DustCircle(npc.Center, 16, 10, DustID.Shadowflame, 5f);
                }

                AttackTimer++;
            }


            AITimer++;
        }

        public override void OnKill(NPC npc)
        {
            if (!NPC.downedPlantBoss) WorldGenSystem.PlaceDungeonPactTiles();
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Venom, 90);
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {

        }

        public override bool PreAI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return true;
            return true;
        }

        public override void PostAI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        }
    }
}
