using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.NPCs.Underworld
{
    public class UnderworldNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.FireImp
                || entity.type == NPCID.Demon
                || entity.type == NPCID.VoodooDemon
                || entity.type == NPCID.LavaSlime
                || entity.type == NPCID.Hellbat;
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (EventSystem.hellbreachActive && player.ZoneOverworldHeight)
            {
                spawnRate = (int)(spawnRate * 0.3f);
                maxSpawns = (int)(maxSpawns * 10f);
            }    
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && EventSystem.hellbreachActive)
            {
                pool.Clear();
                pool.Add(NPCID.LavaSlime, 0.4f);
                pool.Add(NPCID.Hellbat, 0.35f);
                pool.Add(NPCID.FireImp, 0.25f);
                pool.Add(NPCID.Demon, 0.15f);
                pool.Add(NPCID.VoodooDemon, 0.05f);
            }
        }
    }
}
