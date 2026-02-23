using Terraria.DataStructures;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds;

public class TheHungry : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.TheHungry || entity.type == NPCID.TheHungryII;
    }

    public override void AI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (AITimer % 600 == 0 && npc.HasValidTarget && AITimer > 0)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.Center.DirectionTo(Main.player[npc.target].Center) * 6, ProjectileID.Fireball, npc.damage / 3, 1f);
            }
        }

        AITimer++;
    }

    public override void OnKill(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (Main.netMode != NetmodeID.MultiplayerClient && npc.type == NPCID.TheHungryII)
        {
            NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu, npc.whoAmI, ai3: 0.3f);
        }
    }
}
