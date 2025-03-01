using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Forest;
using TerrorMod.Content.NPCs.Hostile.Special;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class Destroyer : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        Player player;
        int Attack = 0;
        int AttackTimer = 0;
        Vector2 targetPos = Vector2.Zero;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.TheDestroyer
                || entity.type == NPCID.TheDestroyerBody
                || entity.type == NPCID.TheDestroyerTail;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;

            //if (npc.type == NPCID.TheDestroyerBody)
            //{
            //    //Main.NewText("ai0: " + npc.ai[0]);
            //    //Main.NewText("ai1: " + npc.ai[1]);
            //    //Main.NewText("ai2: " + npc.ai[2]);
            //    //Main.NewText("ai3: " + npc.ai[3]);
            //}

            if (AITimer % 600 == 0)
            {
                Attack++;
                if (Attack > 3) Attack = 0;
                AttackTimer = 0;
            }

            switch (npc.type)
            {
                case NPCID.TheDestroyer:
                    player = Main.player[npc.target];
                    HeadAI(npc);
                    break;
                case NPCID.TheDestroyerBody or NPCID.TheDestroyerTail:
                    BodyAI(npc);
                    break;
            }

            AITimer++;
        }

        enum HeadAttacks
        {
            Default,
            LaserSpam,
            Summon,
            Follow
        }

        void HeadAI(NPC npc)
        {
            if (npc.life < npc.lifeMax / 10)
            {
                npc.velocity = npc.DirectionTo(player.Center) * 4;

                if (AITimer % 5 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 dir = npc.DirectionTo(player.Center).RotatedBy(MathHelper.ToRadians(Main.rand.Next(-15, 15)));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 7, ProjectileID.DeathLaser, npc.damage / 5, 1f);
                    }
                }

                return;
            }
            switch (Attack)
            {
                case (int)HeadAttacks.Default:
                    break;
                case (int)HeadAttacks.LaserSpam:
                    LaserSpam(npc);
                    break;
                case (int)HeadAttacks.Summon:
                    SummonHead(npc);
                    break;
                case (int)HeadAttacks.Follow:
                    Follow(npc);
                    break;
            }
        }

        void LaserSpam(NPC npc)
        {
            npc.velocity = npc.DirectionTo(player.Center) * 4;
            if (AttackTimer > 180)
            {
                if (AITimer % 10 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 dir = npc.DirectionTo(player.Center).RotatedBy(MathHelper.ToRadians(Main.rand.Next(-15, 15)));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 7, ProjectileID.DeathLaser, npc.damage / 5, 1f);
                    }
                }
            }
            else
            {
                if (AITimer % 30 == 0)
                {
                    LemonUtils.DustCircle(npc.Center, 16, 10, DustID.GemRuby);
                    SoundEngine.PlaySound(SoundID.Roar, npc.Center);
                }
            }
            AttackTimer++;
        }

        void SummonHead(NPC npc)
        {
            npc.velocity /= 1.05f;
        }

        void Follow(NPC npc)
        {
            npc.velocity = npc.DirectionTo(player.Center) * 8;
        }

        void BodyAI(NPC npc)
        {
            if (npc.life < npc.lifeMax / 10)
            {
                return;
            }
            switch (Attack)
            {
                case (int)HeadAttacks.Default:
                    break;
                case (int)HeadAttacks.LaserSpam:

                    break;
                case (int)HeadAttacks.Summon:
                    SummonBody(npc);
                    break;
                case (int)HeadAttacks.Follow:
                    break;
            }
        }

        void SummonBody(NPC npc)
        {
            if (AttackTimer > 0 && AttackTimer % 200 == 0 && npc.ai[0] % 16 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<BombSlime>());
                }
            }
            AttackTimer++;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (Main.npc.Any(n => n.active && n.type == ModContent.NPCType<MechanicalCore>()))
            {
                modifiers.FinalDamage *= 0;
            }

            if (AITimer < 300)
            {
                modifiers.FinalDamage *= 0.05f;
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<FearDebuff>(), 60);
        }
    }
}
