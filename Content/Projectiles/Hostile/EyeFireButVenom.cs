using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Projectiles.Hostile;

public class EyeFireButVenom : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.EyeFire;

    ref float Amount => ref Projectile.ai[1];
    ref float Speed => ref Projectile.ai[2];

    public override void SetDefaults()
    {
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.hostile = true;
        Projectile.tileCollide = false;
        Projectile.aiStyle = 0;
        Projectile.timeLeft = 20;
        Projectile.Opacity = 0f;
        Projectile.extraUpdates = 2;
    }

    public override void AI()
    {
        if (Amount <= 0) Amount = 1;
        if (Speed <= 0) Speed = 1;
        LemonUtils.DustCircle(Projectile.Center, (int)Amount, Speed, DustID.Shadowflame, Main.rand.NextFloat(1.5f, 3f));
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        target.AddBuff(BuffID.Venom, 120);
    }
}
