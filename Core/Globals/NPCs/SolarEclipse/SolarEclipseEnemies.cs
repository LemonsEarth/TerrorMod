using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Forest;

namespace TerrorMod.Core.Globals.NPCs.FrostMoonEnemies
{
    public class SolarEclipseEnemies : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Eyezor
                || entity.type == NPCID.SwampThing
                || entity.type == NPCID.Frankenstein
                || entity.type == NPCID.Vampire
                || entity.type == NPCID.VampireBat
                || entity.type == NPCID.CreatureFromTheDeep
                || entity.type == NPCID.Mothron
                || entity.type == NPCID.MothronEgg
                || entity.type == NPCID.MothronSpawn
                || entity.type == NPCID.Reaper
                || entity.type == NPCID.Fritz
                || entity.type == NPCID.ThePossessed
                || entity.type == NPCID.DeadlySphere
                || entity.type == NPCID.Nailhead
                || entity.type == NPCID.Psycho
                || entity.type == NPCID.DrManFly
                || entity.type == NPCID.Butcher;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Burning, 60);
            }

            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Frostburn, 90);
            }

            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Obstructed, 120);
            }

            if (Main.rand.NextBool(3))
            {
                target.AddBuff(ModContent.BuffType<FearDebuff>(), 180);
            }
        }

        public override void AI(NPC npc)
        {
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            switch (npc.type)
            {
                case NPCID.Frankenstein:
                    if (AITimer % 120 == 0 && npc.life < npc.lifeMax)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            npc.HealEffect(npc.lifeMax / 5);
                            npc.life += npc.lifeMax / 5;
                        }
                        npc.netUpdate = true;
                    }
                    break;
                case NPCID.Eyezor:
                    if (npc.ai[2] < 120)
                    {
                        npc.ai[2] = 120;
                    }
                    break;
                case NPCID.DrManFly:
                    if ((npc.ai[1] > 40 || npc.ai[1] < 20) && npc.ai[1] > 0)
                    {
                        npc.ai[1] = 40;
                    }
                    break;
                case NPCID.Reaper:
                    if (AITimer % 60 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.DirectionTo(player.Center) * 5, ProjectileID.DemonSickle, npc.damage / 4, 1f);
                        }
                    }
                    break;
                case NPCID.DeadlySphere:
                    if (AITimer % 100 == 0)
                    {
                        npc.velocity = npc.DirectionTo(player.Center) * 25;
                    }
                    break;
                case NPCID.Mothron:
                    if (npc.ai[0] > 3 && npc.ai[0] < 4)
                    {
                        if (AITimer % 30 == 0)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.MothronEgg);
                            }
                        }
                    }
                    break;
                case NPCID.Psycho:
                    npc.Opacity = 0.1f;
                    break;
            }

            AITimer++;
        }

        public override void PostAI(NPC npc)
        {
            if (npc.type == NPCID.MothronEgg)
            {
                if (npc.ai[0] < 540) npc.ai[0] = 540;
            }
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

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (npc.type == NPCID.MothronEgg) modifiers.FinalDamage *= 0.25f;
            if (npc.type == NPCID.Mothron && npc.ai[0] > 4f) modifiers.FinalDamage *= 0.2f; 
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            
        }
    }
}
