using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;

namespace TerrorMod.Content.Projectiles.Hostile;

public class ExplosionLarge : ModProjectile
{
    int AITimer = 0;

    public override void SetStaticDefaults()
    {
        Main.projFrames[Projectile.type] = 10;
    }

    public override void SetDefaults()
    {
        Projectile.width = 64;
        Projectile.height = 64;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 40;
        Projectile.alpha = 0;
        Projectile.scale = 2.5f;
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        target.AddBuff(BuffID.Bleeding, 180);
        target.AddBuff(BuffID.Slow, 30);
        target.AddBuff(BuffID.Weak, 240);
    }

    public override void AI()
    {
        if (AITimer == 0)
        {
            Projectile.rotation = MathHelper.ToRadians(Main.rand.NextFloat(0, 360));
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 2f, PitchRange = (-0.2f, 0.2f)}, Projectile.Center);
        }
        Projectile.frameCounter++;
        if (Projectile.frameCounter == 4)
        {
            Projectile.frame++;
            Projectile.frameCounter = 0;
            if (Projectile.frame >= 10)
            {
                Projectile.frame = 0;
            }
        }

        Lighting.AddLight(Projectile.Center, 3, 3, 3);
        AITimer++;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Vector2 drawOrigin = texture.Frame(1, 10, 0, Projectile.frame).Size() * 0.5f;
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(1, 10, 0, Projectile.frame), Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None);
        return false;
    }
}
