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
    public class InfiniteTerrorHeadProj : ModProjectile
    {
        int AITimer = 0;
        ref float Rotation => ref Projectile.ai[0];
        ref float TimeToFire => ref Projectile.ai[1];
        ref float Speed => ref Projectile.ai[2];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 150;
            DrawOffsetX = -30;
            DrawOriginOffsetY = -16;
        }

        public override void AI()
        {
            if (AITimer == 0)
            {
                LemonUtils.DustCircle(Projectile.Center, 32, 15, DustID.GemDiamond, 4f);
                LemonUtils.DustCircle(Projectile.Center, 32, 10, DustID.GemDiamond, 4f);
                LemonUtils.DustCircle(Projectile.Center, 32, 5, DustID.GemDiamond, 4f);
                SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
            }
            Projectile.rotation = Rotation;

            if (AITimer < TimeToFire)
            {
                Projectile.velocity = -Vector2.UnitX.RotatedBy(Projectile.rotation);
            }
            else
            {
                Projectile.velocity = Vector2.UnitX.RotatedBy(Projectile.rotation) * Speed;
            }

            AITimer++;
        }

        public override void OnKill(int timeLeft)
        {
            
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f + DrawOriginOffsetX, Projectile.height * 0.5f + DrawOriginOffsetY);
            for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(DrawOffsetX, 0);
                Color color = Projectile.GetAlpha(lightColor * 0.5f) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
