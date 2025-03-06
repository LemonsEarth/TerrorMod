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

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class DukeFishron : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        int AttackTimer = 0;
        Vector2 savedPos = Vector2.Zero;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.DukeFishron;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {

        }

        public override bool PreAI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return true;
            return true;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            if (AITimer % 60 == 0 && npc.life + 1000 < npc.lifeMax)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.HealEffect(200);
                    npc.life += 200;
                }
                npc.netUpdate = true;
            }
            switch (npc.ai[0]) //attack
            {
                case 1: // dashing
                    npc.velocity *= 1.025f;
                    break;
                case 6: // dashing
                    npc.velocity *= 1.035f;
                    break;
                case 8: // sharknadoes p2
                    if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] == 60)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 pos = player.Center + Vector2.UnitY.RotatedBy(i * MathHelper.PiOver2) * 500;
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.Zero, ProjectileID.SharknadoBolt, npc.damage / 4, 1f, ai1: 1);
                        }
                    }
                    break;
                case 9: // p3 transition
                    if (npc.ai[2] == 0 || npc.ai[2] == 170)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int healAmount = (int)(npc.lifeMax * 0.25f) - npc.life;
                            npc.HealEffect(healAmount);
                            npc.life += healAmount;
                        }
                        npc.netUpdate = true;
                    }
                    break;
                case 11: // dashing p3
                    if (npc.ai[2] == 5)
                    {
                        npc.velocity *= 1.3f;
                        if (npc.ai[3] == 3)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileID.SharknadoBolt, npc.damage / 4, 1f, ai1: 1);
                            }
                        }
                    }
                    break;
                case 10:
                    if (npc.ai[3] == 0)
                    {
                        if (npc.ai[2] == 27)
                        {
                            SoundEngine.PlaySound(SoundID.Thunder);
                            SoundEngine.PlaySound(SoundID.Thunder);
                            SoundEngine.PlaySound(SoundID.Thunder);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = -8; i <= 8; i++)
                                {
                                    Vector2 pos = savedPos + new Vector2(i * 300, -1000);
                                    Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.UnitY * 15, ProjectileID.CultistBossLightningOrbArc, npc.damage / 4, 1f, ai0: MathHelper.PiOver2);
                                }
                            }
                        }
                        else if (npc.ai[2] == 0)
                        {
                            savedPos = player.Center;
                            for (int i = -8; i <= 8; i++)
                            {
                                Vector2 pos = player.Center + new Vector2(i * 300, 0);
                                LemonUtils.DustCircle(pos, 16, 2f, DustID.GemDiamond, 2f);
                            }
                        }
                    }
                    break;
            }
            AITimer++;
        }

        public override void PostAI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;

            modifiers.FinalDamage *= 0.9f;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Cursed, 75);
            target.AddBuff(BuffID.Weak, 120);
        }
    }
}
