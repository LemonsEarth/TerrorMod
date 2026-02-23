using System.Collections.Generic;

namespace TerrorMod.Core.Globals.NPCs.Pillars;

public class StardustEnemies : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.StardustCellBig
            || entity.type == NPCID.StardustJellyfishBig
            || entity.type == NPCID.StardustSoldier
            || entity.type == NPCID.StardustSpiderBig;
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
        if (NPC.AnyNPCs(NPCID.LunarTowerStardust))
        {
            pool.Add(NPCID.StardustJellyfishBig, 0.1f);
            pool.Add(NPCID.StardustSoldier, 0.1f);
            pool.Add(NPCID.StardustSpiderBig, 0.1f);
            pool.Add(NPCID.StardustCellBig, 0.1f);
        }
    }
}
