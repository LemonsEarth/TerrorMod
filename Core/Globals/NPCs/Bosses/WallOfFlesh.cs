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
using TerrorMod.Core.Systems;
using TerrorMod.Content.Buffs.Buffs;

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
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
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
            int attackRate = npc.life < npc.lifeMax * 0.5f ? 600 : 300;
            if (AITimer % attackRate == 0 && AITimer > 0)
            {
                doFlamethrower = !doFlamethrower;
                snappedPosition = player.Center;
                SoundEngine.PlaySound(SoundID.NPCDeath10 with { Volume = 2f, PitchRange = (-0.6f, -0.3f), }, npc.Center);
            }

            if (doFlamethrower)
            {
                if (AITimer % 5 == 0)
                {
                    float rot = npc.direction == 1 ? 0 : MathHelper.Pi;
                    npc.rotation = Utils.AngleLerp(rot, npc.Center.DirectionTo(snappedPosition).ToRotation(), (AITimer % attackRate) / (float)attackRate);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitX.RotatedBy(npc.rotation) * 20, ModContent.ProjectileType<EyeFireButFire>(), npc.damage / 5, 1f);
                    }
                }
            }
            else
            {
                if (AITimer % 120 == 0 && npc.life < npc.lifeMax * 0.5f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 position = player.Center + new Vector2(npc.direction * Main.rand.Next(450, 750), Main.rand.Next(-500, 500));
                        int above = position.Y > player.Center.Y ? 1 : 0;
                        Projectile.NewProjectile(npc.GetSource_FromAI(), position, Vector2.Zero, ModContent.ProjectileType<HungryCannon>(), npc.damage / 5, 1f, ai1: above);
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

        bool placedAll = false;
        public override void OnKill(NPC npc)
        {
            if (Main.hardMode || placedAll) return;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int maxCores = !Main.masterMode ? 4 : 8;
                for (int i = 0; i < maxCores; i++)
                {
                    int counter = 0;
                    while (counter < 1000)
                    {
                        bool placed = false;
                        int x = WorldGen.genRand.Next(500, Main.maxTilesX - 500);
                        int y = WorldGen.genRand.Next(400, Main.maxTilesY - 400);
                        int minDistance = !Main.masterMode ? 5000 : 2000;
                        if (!Main.tile[x, y].HasTile && Main.tile[x, y].WallType == 0
                            && !Main.npc.Any(n => n.active && n.type == ModContent.NPCType<MechanicalCore>() && n.Center.Distance(new Vector2(x * 16, y * 16)) < minDistance))
                        {
                            NPC.NewNPC(npc.GetSource_FromAI(), x * 16, y * 16, ModContent.NPCType<MechanicalCore>());

                            placed = true;
                        }
                        counter++;
                        if (placed) break;
                    }
                }
                placedAll = true;
            }
            foreach (var player in Main.ActivePlayers)
            {
                player.AddBuff(ModContent.BuffType<BeforeTheStormBuff>(), 36000);
            }

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
