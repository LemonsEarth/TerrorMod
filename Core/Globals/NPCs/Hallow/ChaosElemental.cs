namespace TerrorMod.Core.Globals.NPCs.Hallow;

public class ChaosElemental : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.ChaosElemental;
    }

    public override void AI(NPC npc)
    {
        if (npc.ai[3] < 170) npc.ai[3] = 170;
        npc.ai[3]++;
    }
}
