using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds;

public class PhantasmDragon : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.CultistDragonBody1
            || entity.type == NPCID.CultistDragonBody2
            || entity.type == NPCID.CultistDragonBody3
            || entity.type == NPCID.CultistDragonBody4
            || entity.type == NPCID.CultistDragonHead
            || entity.type == NPCID.CultistDragonTail;
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (npc.type == NPCID.CultistDragonBody1
            || npc.type == NPCID.CultistDragonBody2
            || npc.type == NPCID.CultistDragonBody3
            || npc.type == NPCID.CultistDragonBody4)
        {
            modifiers.FinalDamage *= 0.33f;
        }
    }
}
