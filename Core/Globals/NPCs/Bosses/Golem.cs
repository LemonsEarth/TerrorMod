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
using Terraria.GameContent.ItemDropRules;
using TerrorMod.Content.Items.Accessories;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class Golem : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        int AttackTimer = 0;

        bool EitherArm(NPC npc)
        {
            return npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight;
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Golem
                || entity.type == NPCID.GolemHead
                || entity.type == NPCID.GolemHeadFree
                || entity.type == NPCID.GolemFistLeft
                || entity.type == NPCID.GolemFistRight;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            if (EitherArm(npc))
            {
                if (npc.ai[1] < 58 && npc.ai[1] > 0) npc.ai[1] += 3;
            }

            else if (npc.type == NPCID.GolemHeadFree)
            {
                if (AttackTimer > 300 && AttackTimer < 600)
                {
                    if (AttackTimer % 60 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int dir = Main.rand.NextBool().ToDirectionInt();
                            Vector2 pos = player.Center + Vector2.UnitX * dir * 250;
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<DartTrap>(), npc.damage / 4, 1, ai1: -dir);
                        }
                    }
                }
                if (AITimer % 600 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY * 2, ProjectileID.CultistBossLightningOrb, npc.damage / 4, 1);
                    }
                }
                if (AttackTimer > 600) AttackTimer = 0;
                AttackTimer++;
            }


            AITimer++;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (npc.type == NPCID.Golem)
            {
                modifiers.FinalDamage *= 0.5f;
            }

            else if (EitherArm(npc))
            {
                modifiers.FinalDamage *= 0.5f;
            }

            else if (npc.type == NPCID.GolemHead)
            {
                modifiers.FinalDamage *= 0.66f;
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Weak, 120);
            target.AddBuff(BuffID.WitheredArmor, 120);
            if (Main.rand.NextBool(4)) target.AddBuff(ModContent.BuffType<Weight>(), 120);
        }
    }
}
