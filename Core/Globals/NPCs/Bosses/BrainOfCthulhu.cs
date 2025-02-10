using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class BrainOfCthulhu : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        int phase2Timer = 0;
        bool teleportationSpam = false;

        public override void SetDefaults(NPC entity)
        {
            entity.lifeMax = 1600;
            entity.defense = 16;
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.BrainofCthulhu;
        }

        public override void AI(NPC npc)
        {
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            if (npc.localAI[2] > 0) // if in phase 2
            {
                if (npc.localAI[1] == 0) SoundEngine.PlaySound(SoundID.Item105 with { PitchRange = (0.4f, 6f), Volume = 1f }, npc.Center);
                if (teleportationSpam)
                {
                    if (npc.localAI[1] < 75) // tp timer
                    {
                        npc.localAI[1] = 75;
                    }
                }
                else if (phase2Timer % 30 == 0 && phase2Timer > 60)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath13, npc.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.Center.DirectionTo(player.Center) * 10, ModContent.ProjectileType<CrimsonCarrierProj>(), npc.damage / 3, 1f);
                    }
                }

                //Main.NewText("ai0: " + npc.ai[0]);
                //Main.NewText("ai1: " + npc.ai[1]);
                //Main.NewText("ai2: " + npc.ai[2]);
                //Main.NewText("ai3: " + npc.ai[3]);

                if (phase2Timer == 180 && teleportationSpam)
                {
                    teleportationSpam = false;
                    phase2Timer = 0;
                }

                if (phase2Timer == 360)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath10, npc.Center);
                    teleportationSpam = true;
                    phase2Timer = 0;
                }
                phase2Timer++;
            }

            AITimer++;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<FearDebuff>(), 60);
        }
    }
}
