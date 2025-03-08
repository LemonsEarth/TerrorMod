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
using System;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class MoonLord : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        int AttackTimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.MoonLordCore
                || entity.type == NPCID.MoonLordHand
                || entity.type == NPCID.MoonLordHead;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            if (npc.type == NPCID.MoonLordCore)
            {
                if (AITimer == 540)
                {
                    Vector2 pos = npc.Center;
                    LemonUtils.DustCircle(pos, 32, 15, DustID.GemDiamond, 4f);
                    LemonUtils.DustCircle(pos, 32, 10, DustID.GemDiamond, 4f);
                    LemonUtils.DustCircle(pos, 32, 5, DustID.GemDiamond, 4f);
                }
                if (AITimer >= 600)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 pos = player.Center + new Vector2(Math.Sign(player.velocity.X) * 300, -1500);
                        Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<DoomSphere>(), 50, 2f);
                    }
                    AITimer = 0;
                }
            }
            else if (npc.type == NPCID.MoonLordHand)
            {
                if (npc.life <= npc.lifeMax * 0.25f)
                {
                    if (npc.ai[0] != 2)
                    {
                        npc.ai[1] += 4;
                    }
                    if (npc.ai[0] == 2)
                    {
                        if (npc.ai[1] < 300)
                        {
                            npc.ai[1] += 7;
                        }
                    }
                }
                else if (npc.life <= npc.lifeMax * 0.5f)
                {
                    if (npc.ai[0] != 2)
                    {
                        npc.ai[1] += 3;
                    }
                    if (npc.ai[0] == 2)
                    {
                        if (npc.ai[1] < 300)
                        {
                            npc.ai[1] += 3;
                        }
                    }
                }
                else if (npc.life <= npc.lifeMax * 0.75f)
                {
                    if (npc.ai[0] != 2)
                    {
                        npc.ai[1] += 1;
                    }
                    if (npc.ai[0] == 2)
                    {
                        if (npc.ai[1] < 300)
                        {
                            npc.ai[1] += 1;
                        }
                    }
                }
            }

            AITimer++;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;

        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {

        }
    }
}
