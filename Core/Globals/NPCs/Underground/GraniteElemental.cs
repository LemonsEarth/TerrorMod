namespace TerrorMod.Core.Globals.NPCs.Underground;

public class GraniteElemental : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.GraniteFlyer;
    }

    public override void PostAI(NPC npc)
    {
        npc.noTileCollide = true;
    }
}
