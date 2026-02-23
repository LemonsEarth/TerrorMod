using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.NPCs;

public class AllGlobalNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return true;
    }

    public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
    {
        if (!SkullSystem.blindSkullActive) return null;
        return false;
    }

    public override void OnKill(NPC npc)
    {
        if (SkullSystem.toughLuckSkullActive && Main.rand.NextBool(4))
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Vector2.Zero, ProjectileType<ExplosionLarge>(), 20, 1f);
            }
        }
    }
}
