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
using TerrorMod.Content.NPCs.Hostile.Special;
using Terraria.Graphics.Effects;
using System;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class EmpressOfLight : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.HallowBoss;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {

        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            switch (npc.ai[0]) //attack
            {
                case 2: // projspam
                    if (npc.ai[1] % 30 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 pos = player.Center + new Vector2(Math.Sign(player.velocity.X) * Main.rand.Next(600, 800), Main.rand.Next(-200, 200));
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(10, 10), ProjectileID.HallowBossLastingRainbow, npc.damage / 4, 1f, ai0: 6.6f);
                        }
                    }
                    break;
                case 6: // SunDance
                    if (npc.ai[1] % 60 == 0 && npc.ai[1] > 0)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                float addRot = 360f / npc.ai[1];
                                Vector2 pos = player.Center + Vector2.UnitY.RotatedBy(MathHelper.ToRadians(60 + addRot * 15) * i) * 300;
                                Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.Zero, ProjectileID.FairyQueenLance, npc.damage / 4, 1f, ai0: pos.DirectionTo(player.Center).ToRotation());
                            }
                        }
                    }
                    break;
                case 7: // blade spam
                    if (npc.ai[1] % 60 == 0 && npc.ai[1] > 0)
                    {
                        for (int i = -3; i <= 3; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 pos = player.Center + new Vector2(-800, i * 150);
                                Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.Zero, ProjectileID.FairyQueenLance, npc.damage / 4, 1f);
                            }
                        }
                    }
                    break;
                case 9: // dash
                    if (npc.ai[1] % 10 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileID.HallowBossRainbowStreak, npc.damage / 4, 1f);
                        }
                    }
                    break;
            }

            AITimer++;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;

            modifiers.FinalDamage *= 0.8f;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(4))
            {
                target.AddBuff(BuffID.OnFire, 120);
                target.AddBuff(BuffID.Frostburn, 120);
                target.AddBuff(BuffID.CursedInferno, 120);
                target.AddBuff(BuffID.Ichor, 120);
                target.AddBuff(BuffID.ShadowFlame, 120);
            }
        }
    }
}
