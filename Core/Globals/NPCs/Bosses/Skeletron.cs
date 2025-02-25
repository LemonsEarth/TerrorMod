using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;
using System.Collections.Generic;
using System.Linq;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class Skeletron : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        int AttackTimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.SkeletronHead;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            //ai1 is spinning
            //ai2 is a timer
            //ai3 will be used for attack phases when hands are dead

            int handCount = Main.npc.Count(npc => npc.active && npc.type == NPCID.SkeletronHand);
            if (handCount > 0)
            {
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }

            if (handCount == 0)
            {
                if (AITimer % 600 == 0)
                {
                    SwitchPhase(npc);
                }

                switch (npc.ai[3])
                {
                    case 0:
                        if (AttackTimer % 120 == 0 && AttackTimer > 60)
                        {
                            int amount = npc.life < npc.lifeMax * 0.75f ? 2 : 1;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = 0; i < 8 * amount; i++)
                                {
                                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedBy(i * (MathHelper.PiOver4 / amount)) * 5, ProjectileID.LostSoulHostile, npc.damage / 4, 1f, ai0: player.Center.X, ai1: player.Center.Y);
                                }
                            };
                        }
                        break;
                    case 1:
                        if (AITimer % 60 == 0)
                        {
                            LemonUtils.DustCircle(npc.Center, 16, 10, DustID.GemAmethyst, 2);
                        }
                        if (AttackTimer % 120 == 0 && AttackTimer > 60)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedBy(i * MathHelper.PiOver4) * 10, ProjectileID.ShadowBeamHostile, npc.damage / 4, 1f, ai1: player.Center.X, ai2: player.Center.Y);
                                }
                            }
                        }
                        break;
                    case 2:
                        if (AttackTimer % 240 == 0)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int amount = Main.masterMode ? 8 : 4;
                                for (int i = 0; i < amount; i++)
                                {
                                    Vector2 pos = Main.player[npc.target].Center + (Vector2.UnitY * 600).RotatedBy(i * (MathHelper.Pi / (amount / 2)));
                                    LemonUtils.DustCircle(pos, 8, 5, DustID.Granite, 3f);
                                    NPC slime = NPC.NewNPCDirect(npc.GetSource_FromAI(), (int)pos.X, (int)pos.Y, NPCID.CursedSkull);
                                }
                            }
                        }
                        break;
                    case 3:
                        if (AttackTimer % 120 == 0 && AttackTimer > 60)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = -2; i <= 2; i++)
                                {
                                    Projectile.NewProjectile(npc.GetSource_FromAI(), player.Center + new Vector2(i * 48, 900), -Vector2.UnitY * 15, ProjectileID.InfernoHostileBolt, npc.damage / 4, 1f);
                                }
                            };
                        }

                        if (AttackTimer % 60 < 10 && AttackTimer % 2 == 0)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.Center.DirectionTo(player.Center) * 10, ProjectileID.Skull, npc.damage / 4, 1f);
                            };
                        }
                        break;
                }
                AttackTimer++;
            }

            //Main.NewText("ai0: " + npc.ai[0]);
            //Main.NewText("ai1: " + npc.ai[1]);
            //Main.NewText("ai2: " + npc.ai[2]);
            //Main.NewText("ai3: " + npc.ai[3]);
            AITimer++;
        }

        void SwitchPhase(NPC npc)
        {
            npc.ai[3]++;
            if (npc.life > npc.lifeMax * 0.5f)
            {
                if (npc.ai[3] >= 3)
                {
                    npc.ai[3] = 0;
                }
            }
            else
            {
                if (npc.ai[3] >= 4)
                {
                    npc.ai[3] = 0;
                }
            }
            switch (npc.ai[3])
            {
                case 0:
                    LemonUtils.DustCircle(npc.Center, 16, 10, DustID.GemDiamond, 2);
                    break;
                case 1:
                    LemonUtils.DustCircle(npc.Center, 16, 10, DustID.GemAmethyst, 2);
                    break;
                case 2:
                    LemonUtils.DustCircle(npc.Center, 16, 10, DustID.Granite, 2);
                    break;
            }
            AttackTimer = 0;
            npc.netUpdate = true;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (npc.ai[1] == 1) modifiers.FinalDamage *= 0.5f; 
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Cursed, 75);
            target.AddBuff(BuffID.Weak, 120);
        }
    }
}
