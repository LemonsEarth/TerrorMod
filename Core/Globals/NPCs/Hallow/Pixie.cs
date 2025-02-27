﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorMod.Core.Globals.NPCs.Hallow
{
    public class Pixie : GlobalNPC
    {
        int AITimer = 0;
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Pixie;
        }

        public override void AI(NPC npc)
        {
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            if (AITimer % 600 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileID.HallowBossRainbowStreak, npc.damage / 4, 1f);
                }
            }
            AITimer++;
        }
    }
}
