using Terraria.DataStructures;

namespace TerrorMod.Core.Globals.Projectiles.Hostile;

public class EyeLaser : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    bool sourceIsRetinazer = false;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return entity.type == ProjectileID.EyeLaser;
    }

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (source is EntitySource_Parent parent && parent.Entity is NPC npc && npc.type == NPCID.Retinazer)
        {
            projectile.velocity *= 0.1f;
            sourceIsRetinazer = true;
        }
    }

    public override void AI(Projectile projectile)
    {
        if (sourceIsRetinazer)
        {
            projectile.velocity *= 1.02f;
        }
    }
}
