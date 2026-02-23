using Terraria.Audio;

namespace TerrorMod.Content.Projectiles.Hostile;

public class DartHostile : ModProjectile
{
    int AITimer = 0;

    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 2;
    }

    public override void SetDefaults()
    {
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 120;
    }

    public override void AI()
    {
        if (AITimer == 0)
        {
            SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, Projectile.Center);
        }
        Projectile.tileCollide = false;
        Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.GemTopaz).noGravity = true;
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

        Projectile.frameCounter++;
        if (Projectile.frameCounter == 12)
        {
            Projectile.frame++;
            Projectile.frameCounter = 0;
            if (Projectile.frame >= 2)
            {
                Projectile.frame = 0;
            }
        }

        AITimer++;
    }

    public override void OnKill(int timeLeft)
    {
        
    }
}
