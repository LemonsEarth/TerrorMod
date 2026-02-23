namespace TerrorMod.Content.Projectiles.Hostile;

public class EyeFireButFire : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.EyeFire;

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.EyeFire);
        Projectile.tileCollide = false;
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        target.AddBuff(BuffID.Burning, 120);
    }
}
