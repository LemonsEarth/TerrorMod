using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
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
    public class LaserSkull : ModProjectile
    {
        public override string Texture => "TerrorMod/Common/Assets/Textures/EmptyPixel";

        string LeftSkullPath => "TerrorMod/Content/Projectiles/Hostile/InfiniteTerrorSkullLeft";
        string RightSkullPath => "TerrorMod/Content/Projectiles/Hostile/InfiniteTerrorSkullRight";

        static Asset<Texture2D> LeftSkull;
        static Asset<Texture2D> RightSkull;
        int AITimer = 0;

        ref float WaitTime => ref Projectile.ai[0];
        ref float MoveDistance => ref Projectile.ai[2];
        ref float Rotation => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            LeftSkull = ModContent.Request<Texture2D>(LeftSkullPath);
            RightSkull = ModContent.Request<Texture2D>(RightSkullPath);
        }

        public override void SetDefaults()
        {
            Projectile.width = 97;
            Projectile.height = 97;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
        }

        float maxDistance = 50;
        public override void AI()
        {
            if (AITimer == 0)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath13, Projectile.Center);
            }

            if (AITimer > WaitTime)
            {
                if (MoveDistance < maxDistance)
                {
                    MoveDistance += (MoveDistance + 1) * 0.8f;
                }
                Projectile.Opacity = MathHelper.Lerp(Projectile.Opacity, 0, 1f / 10f);
            }
            Projectile.rotation = Rotation;
            Projectile.velocity = Vector2.Zero;

            AITimer++;
        }

        public override void OnKill(int timeLeft)
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D leftTexture = LeftSkull.Value;
            Texture2D rightTexture = RightSkull.Value;
            Vector2 drawOrigin = leftTexture.Size() * 0.5f;
            Vector2 direction = Vector2.UnitX.RotatedBy(Rotation);
            Vector2 perpendicular = direction.RotatedBy(MathHelper.PiOver2);
            Vector2 leftOffset = perpendicular * ((leftTexture.Height / 2) + MoveDistance);
            Vector2 rightOffset = perpendicular * ((rightTexture.Height / 2) + MoveDistance);
            Vector2 leftPos = Projectile.Center - leftOffset;
            Vector2 rightPos = Projectile.Center + rightOffset;

            Main.EntitySpriteDraw(leftTexture, leftPos - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.FlipHorizontally);
            Main.EntitySpriteDraw(rightTexture, rightPos - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.FlipHorizontally);

            return false;
        }
    }
}
