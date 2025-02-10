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
    public class EyeFireClone : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.EyeFire;

        ref float AITimer => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EyeFire);
            AIType = ProjectileID.EyeFire;
            Projectile.tileCollide = false;
        }
    }
}
