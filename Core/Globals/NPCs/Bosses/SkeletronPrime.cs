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

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class SkeletronPrime : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        int AttackTimer = 0;
        bool AnyArms => NPC.AnyNPCs(NPCID.PrimeCannon) || NPC.AnyNPCs(NPCID.PrimeSaw) || NPC.AnyNPCs(NPCID.PrimeVice) || NPC.AnyNPCs(NPCID.PrimeLaser);

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.SkeletronPrime;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {

        }

        public override bool PreAI(NPC npc)
        {
            if (!AnyArms && npc.ai[1] == 1) return false;
            return true;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            if (!AnyArms)
            {
                if (AITimer % 60 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (npc.life < npc.lifeMax * 0.5f)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedBy(i * MathHelper.PiOver4) * 5, ProjectileID.DeathLaser, npc.damage / 5, 1f);
                            }
                        }
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.Center.DirectionTo(player.Center) * 10, ProjectileID.Skull, npc.damage / 4, 1f);

                    };
                }
            }
            AITimer++;
        }

        public override void PostAI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            if (!AnyArms && npc.ai[1] == 1)
            {
                npc.MoveToPos(player.Center, 0.04f, 0.04f, 1f, 0.6f);

                if (npc.ai[2] % 60 < 10 && npc.ai[2] % 2 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Roar with { Pitch = 1f, Volume = 0.7f, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 1 });

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.Center.DirectionTo(player.Center) * 10, ProjectileID.Skull, npc.damage / 4, 1f);
                    };
                }

                npc.rotation += MathHelper.ToRadians(6);
                npc.ai[2]++;
                if (npc.ai[2] > 300) npc.ai[1] = 0;
            }
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<MechanicalCore>()))
            {
                modifiers.FinalDamage *= 0;
            }

            if (AnyArms)
            {
                modifiers.FinalDamage *= 0.5f;
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Cursed, 75);
            target.AddBuff(BuffID.Weak, 120);
        }
    }
}
