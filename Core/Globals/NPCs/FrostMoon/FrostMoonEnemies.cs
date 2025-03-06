using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Forest;

namespace TerrorMod.Core.Globals.NPCs.FrostMoonEnemies
{
    public class FrostMoonEnemires : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override void SetDefaults(NPC entity)
        {

        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.PresentMimic
                || entity.type == NPCID.Flocko
                || entity.type == NPCID.GingerbreadMan
                || entity.type == NPCID.ZombieElf
                || entity.type == NPCID.ZombieElfBeard
                || entity.type == NPCID.ZombieElfGirl
                || entity.type == NPCID.ElfArcher
                || entity.type == NPCID.ElfCopter
                || entity.type == NPCID.Nutcracker
                || entity.type == NPCID.Yeti
                || entity.type == NPCID.Krampus
                || entity.type == NPCID.Everscream
                || entity.type == NPCID.SantaNK1
                || entity.type == NPCID.IceQueen;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Frozen, 30);
            }

            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Frostburn, 90);
            }

            if (Main.rand.NextBool(3))
            {
                target.AddBuff(ModContent.BuffType<Weight>(), 120);
            }
        }

        public override void AI(NPC npc)
        {
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            if (npc.type == NPCID.ElfArcher)
            {
                if (npc.ai[1] > 60) npc.ai[1] = 60;
                if (npc.ai[1] < 30 && npc.ai[1] > 0) npc.ai[1] = 0;
            }

            else if (npc.type == NPCID.ElfCopter)
            {
                if (AITimer % 600 == 0 && AITimer > 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<BombSlime>());
                    }
                }
            }

            else if (npc.type == NPCID.Flocko)
            {
                if (AITimer % 30 == 0 && npc.ai[0] == 20)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.DirectionTo(player.Center) * 10, ProjectileID.FrostWave, npc.damage / 4, 1f);
                    }
                }

                if (AITimer % 90 == 0 && npc.ai[0] != 20)
                {
                    npc.velocity += npc.Center.DirectionTo(player.Center) * 10;
                }
            }

            else if (npc.type == NPCID.Everscream)
            {
                if (npc.ai[0] == 0)
                {
                    if (npc.ai[1] < 180) npc.ai[1] = 180;
                }
            }

            else if (npc.type == NPCID.SantaNK1)
            {
                if (npc.ai[0] == 0)
                {
                    if (npc.ai[1] < 180) npc.ai[1] = 180;
                }
            }

            else if (npc.type == NPCID.IceQueen)
            {
                if (npc.ai[0] == 2)
                {
                    npc.velocity = npc.Center.DirectionTo(player.Center) * 5;
                }

                if (AITimer % 240 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), player.Center - Vector2.UnitY * 600, Vector2.UnitY * 7, ProjectileID.CultistBossIceMist, npc.damage / 4, 1f, ai1: 1);
                    }
                }
            }

            AITimer++;
        }

        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.Everscream)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.position.X + Main.rand.Next(0, npc.width), (int)npc.position.Y + Main.rand.Next(0, npc.height), ModContent.NPCType<TreeSpirit>());
                    }
                }
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (npc.type == NPCID.Everscream)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.rand.NextBool(20))
                    {
                        NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.position.X + Main.rand.Next(0, npc.width), (int)npc.position.Y + Main.rand.Next(0, npc.height), ModContent.NPCType<TreeSpirit>());
                    }
                }
            }
        }
    }
}
