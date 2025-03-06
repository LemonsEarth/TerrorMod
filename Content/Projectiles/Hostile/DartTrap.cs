using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Common.Utils.Prim;

namespace TerrorMod.Content.Projectiles.Hostile
{
    public class DartTrap : ModProjectile
    {
        ref float AITimer => ref Projectile.ai[0];
        ref float Direction => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 80;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.tileCollide = false;
            if (AITimer == 0)
            {
                Projectile.Opacity = 0f;
                if (Direction == 0) Direction = 1;
            }

            if (Projectile.timeLeft > 120)
            {
                Projectile.Opacity = MathHelper.Lerp(0f, 1f, AITimer / 60);
            }

            if (Projectile.timeLeft < 60)
            {
                Projectile.Opacity = MathHelper.Lerp(1f, 0f, (AITimer - 120) / 60f);
            }

            Projectile.spriteDirection = (int)Direction;

            if (AITimer == 90)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 pos = Projectile.position + new Vector2(24 * Direction, 8) + i * Vector2.UnitY * 16;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, Vector2.UnitX * Direction * 8f, ModContent.ProjectileType<DartHostile>(), Projectile.damage, 1f);
                    }
                }
            }

            Projectile.velocity = Vector2.Zero;
            AITimer++;
        }

        public override void OnKill(int timeLeft)
        {
            
        }
    }
}
