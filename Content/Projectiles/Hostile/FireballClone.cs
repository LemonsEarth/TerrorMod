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
    public class FireballClone : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball; // Use Iron Pickaxe texture cause im lazy

        ref float AITimer => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Fireball);
            Projectile.tileCollide = false;
        }
    }
}
