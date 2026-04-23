using Terraria.DataStructures;

namespace TerrorMod.Content.Projectiles.Hostile;

public class FallingSnowball : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.FallingBlockDoesNotFallThroughPlatforms[Type] = true;
        ProjectileID.Sets.FallingBlockTileItem[Type] = new(TileID.SnowBlock, 0);
    }

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.EbonsandBallFalling);
        Projectile.friendly = false;
    }

    ref float FallThrough => ref Projectile.ai[2];

    public override void OnSpawn(IEntitySource source)
    {
        FallThrough = Main.rand.Next(0, 2);
    }

    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        fallThrough = (FallThrough == 1);
        return true;
    }
}
