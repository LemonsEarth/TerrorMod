using Terraria.Audio;

namespace TerrorMod.Content.Projectiles.Hostile;

public class HungryCannon : ModProjectile
{
    ref float AITimer => ref Projectile.ai[0];
    ref float rotation => ref Projectile.ai[1];

    public override void SetDefaults()
    {
        Projectile.width = 40;
        Projectile.height = 36;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 480;
    }

    public override void AI()
    {
        if (AITimer == 0)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath10 with { PitchRange = (0.3f, 0.6f) }, Projectile.Center);
            LemonUtils.DustCircle(Projectile.Center, 8, 3, DustID.GemRuby, 2f);
        }

        if (Projectile.alpha > 0)
        {
            Projectile.alpha -= 7;
        }

        Projectile.rotation = rotation;

        if (AITimer % 5 == 0)
        {
            if (LemonUtils.NotClient())
            {
                Projectile.NewProjectileDirect(
                    Projectile.GetSource_FromAI(),
                    Projectile.Center,
                    Vector2.UnitX.RotatedBy(rotation) * 25,
                    ProjectileType<HungryFlames>(),
                    Projectile.damage,
                    1f,
                    ai0: 5);
            }
        }

        Projectile.velocity = Vector2.Zero;

        AITimer++;
    }
}
