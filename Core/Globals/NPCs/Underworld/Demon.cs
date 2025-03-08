using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Core.Globals.NPCs.Underworld
{
    public class Demon : GlobalNPC
    {
        int AITimer = 0;

        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Demon
                || entity.type == NPCID.VoodooDemon
                || entity.type == NPCID.RedDevil;
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(npc.GetSource_OnHurt(projectile), npc.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 2, Main.rand.Next(ProjectileID.GreekFire1, ProjectileID.GreekFire3), npc.damage / 4, 1f);
            }
        }
    }
}
