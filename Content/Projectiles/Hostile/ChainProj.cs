using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Projectiles.Hostile;

public class ChainProj : ModProjectile
{
    int AITimer = 0;

    ref float Distance => ref Projectile.ai[0];
    ref float Speed => ref Projectile.ai[1];
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 5;
        ProjectileID.Sets.TrailCacheLength[Type] = 20;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }

    public override void SetDefaults()
    {
        Projectile.width = 64;
        Projectile.height = 64;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 480;
        Projectile.penetrate = -1;
    }

    public override void AI()
    {
        if (AITimer == 0)
        {
            LemonUtils.DustCircle(Projectile.Center, 32, 5, DustID.GemDiamond, 4f);
            SoundEngine.PlaySound(SoundID.NPCDeath13, Projectile.Center);
            Projectile.frame = Main.rand.Next(5);
        }

        float sinMovement = (float)Math.Sin(AITimer / Speed) * Distance;
        Vector2 normal = Projectile.velocity.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero);
        Projectile.Center += normal * sinMovement;
        Projectile.rotation = Projectile.oldPos[1].DirectionTo(Projectile.position).ToRotation();

        if (Projectile.timeLeft < 30)
        {
            Projectile.Opacity = MathHelper.Lerp(0f, 1f, Projectile.timeLeft / 30f);
        }

        AITimer++;
    }

    public override void OnKill(int timeLeft)
    {
        
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;

        Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
        for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
        {
            if (k % 4 == 0) continue;
            Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(DrawOffsetX, 0);
            Color color = Projectile.GetAlpha(lightColor * 0.5f) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
            Main.EntitySpriteDraw(texture, drawPos + drawOrigin, texture.Frame(1, 5, 0, Projectile.frame), color * Projectile.Opacity, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
        }
        return true;
    }
}
