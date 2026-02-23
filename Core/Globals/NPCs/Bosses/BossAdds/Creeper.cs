using Terraria.DataStructures;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds;

public class Creeper : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public override void SetStaticDefaults()
    {
        NPCID.Sets.NeverDropsResourcePickups[NPCID.Creeper] = true;
    }

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.Creeper;
    }

    public override void SetDefaults(NPC entity)
    {
        entity.lifeMax = 60;
    }

    public override void OnKill(NPC npc)
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu, npc.whoAmI, ai3: 0.3f);
        }
    }
}
