using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TerrorMod.Content.NPCs.Hostile.Forest;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Core.Globals.Projectiles.Hostile
{
    public class FallenStar : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ProjectileID.FallingStar;
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (Main.rand.NextBool(20) && Main.hardMode)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, projectile.velocity, ModContent.ProjectileType<MeteorHostile>(), 20, 1f, ai1: 1);
                }
                projectile.Kill();
            }
        }
    }
}
