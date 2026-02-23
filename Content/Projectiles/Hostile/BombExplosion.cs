using Terraria.Audio;

namespace TerrorMod.Content.Projectiles.Hostile;

public class BombExplosion : ModProjectile
{
    public override string Texture => "Terraria/Images/Item_0"; // Use Iron Pickaxe texture cause im lazy

    ref float AITimer => ref Projectile.ai[0];

    public override void SetDefaults()
    {
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;
        Projectile.timeLeft = 2;
    }

    public override void AI()
    { 
        AITimer++;
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        for (int i = 0; i < 6; i++)
        {
            Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), Main.rand.Next(61, 64), Main.rand.NextFloat(0.5f, 2f));
        }
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            int explosionRadius = 5;
            int minTileX = (int)(Projectile.Center.X / 16f - explosionRadius);
            int maxTileX = (int)(Projectile.Center.X / 16f + explosionRadius);
            int minTileY = (int)(Projectile.Center.Y / 16f - explosionRadius);
            int maxTileY = (int)(Projectile.Center.Y / 16f + explosionRadius);
            bool explodeWalls = Projectile.ShouldWallExplode(Projectile.Center, explosionRadius, minTileX, maxTileX, minTileY, maxTileY);
            Projectile.ExplodeTiles(Projectile.Center, explosionRadius, minTileX, maxTileX, minTileY, maxTileY, explodeWalls);
        }   
    }
}
