using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorMod.Content.Projectiles.Hostile
{
    public class MeteorHostile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Meteor1;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Meteor1);
            AIType = ProjectileID.Meteor1;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.hostile = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item89);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BombExplosion>(), 20, 1);
            }
            return true;
        }
    }
}
