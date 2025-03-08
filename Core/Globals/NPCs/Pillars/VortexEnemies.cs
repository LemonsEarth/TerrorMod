using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Forest;
using System.Collections.Generic;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.NPCs.Pillars
{
    public class VortexEnemies : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.VortexSoldier
                || entity.type == NPCID.VortexRifleman
                || entity.type == NPCID.VortexLarva
                || entity.type == NPCID.VortexHornetQueen
                || entity.type == NPCID.VortexHornet;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            
        }

        public override void AI(NPC npc)
        {
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            AITimer++;
        }

        public override void PostAI(NPC npc)
        {
            
        }

        public override void OnKill(NPC npc)
        {
            
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (NPC.AnyNPCs(NPCID.LunarTowerVortex))
            {
                pool.Add(NPCID.VortexLarva, 0.1f);
                pool.Add(NPCID.VortexHornet, 0.1f);
                pool.Add(NPCID.VortexHornetQueen, 0.1f);
                pool.Add(NPCID.VortexSoldier, 0.1f);
                pool.Add(NPCID.VortexRifleman, 0.1f);
            }
        }
    }
}
