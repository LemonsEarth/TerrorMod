namespace TerrorMod.Content.Projectiles.Hostile;

public class FireballClone : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.Fireball);
        Projectile.tileCollide = false;
    }
}
