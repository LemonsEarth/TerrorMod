using Terraria.DataStructures;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds;

public class PlanteraTentacle : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.PlanterasTentacle;
    }

    public override void SetDefaults(NPC entity)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        entity.lifeMax = 500;
    }

    public override void OnKill(NPC npc)
    {
        if (Main.netMode != NetmodeID.MultiplayerClient && npc.type == NPCID.TheHungryII)
        {
            NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu, npc.whoAmI, ai3: 0.3f);
        }
    }
}
