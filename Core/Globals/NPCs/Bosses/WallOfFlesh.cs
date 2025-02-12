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

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class WallOfFlesh : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        int AttackTimer = 0;
        bool doFlamethrower = false;
        Vector2 snappedPosition;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.WallofFlesh;
        }

        public override void AI(NPC npc)
        {
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            // ai1 is a timer

            //Main.NewText("ai0: " + npc.ai[0]);
            //Main.NewText("ai1: " + npc.ai[1]);
            //Main.NewText("ai2: " + npc.ai[2]);
            //Main.NewText("ai3: " + npc.ai[3]);
            //Main.NewText("ai0: " + npc.localAI[0]);
            //Main.NewText("ai1: " + npc.localAI[1]);
            //Main.NewText("ai2: " + npc.localAI[2]);
            //Main.NewText("ai3: " + npc.localAI[3]);
            
            if (AITimer % 300 == 0 && AITimer > 0)
            {
                doFlamethrower = !doFlamethrower;
                snappedPosition = player.Center;
                SoundEngine.PlaySound(SoundID.NPCDeath10 with { Volume = 2f, PitchRange = (-0.6f, -0.3f), }, npc.Center);
            }

            if (npc.life > npc.lifeMax * 0.2f)
            {
                if (doFlamethrower && AITimer % 5 == 0)
                {
                    float rot = npc.direction == 1 ? 0 : MathHelper.Pi;
                    npc.rotation = Utils.AngleLerp(rot, npc.Center.DirectionTo(snappedPosition).ToRotation(), (AITimer % 300) / 300f);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitX.RotatedBy(npc.rotation) * 20, ModContent.ProjectileType<EyeFireButFire>(), npc.damage / 5, 1f);
                    }
                }
            }
            else
            {
                if (AITimer % 5 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitX * npc.direction * 25, ModContent.ProjectileType<EyeFireButFire>(), npc.damage / 5, 1f);
                    }
                }   
            }
            

            for (int i = 0; i < 144; i++)
            {
                Vector2 pos = npc.Center + (Vector2.UnitY * 1450).RotatedBy(MathHelper.ToRadians(2.5f * i));
                var dust = Dust.NewDustDirect(pos, 1, 1, DustID.GemRuby, Scale: 1.5f);
                dust.noGravity = true;
            }

            if (npc.Center.Distance(player.Center) > 1500)
            {
                player.AddBuff(BuffID.Poisoned, 30);
                player.AddBuff(BuffID.OnFire, 30);
                player.AddBuff(BuffID.Frostburn, 30);
                player.AddBuff(BuffID.Burning, 30);
                player.AddBuff(BuffID.Venom, 30);
            }

            AITimer++;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            //if (npc.ai[1] == 1) modifiers.FinalDamage *= 0.5f; 
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<FearDebuff>(), 90);
            target.AddBuff(BuffID.Weak, 120);
        }
    }
}
