using Terraria.DataStructures;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Core.Globals.Projectiles.Hostile;

public class HotProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return entity.type == ProjectileID.EyeFire
            || entity.type == ProjectileID.CursedFlameHostile
            || entity.type == ProjectileID.Fireball
            || entity.type == ProjectileType<FireballClone>()
            || entity.type == ProjectileID.GreekFire1
            || entity.type == ProjectileID.GreekFire2
            || entity.type == ProjectileID.GreekFire3
            || entity.type == ProjectileID.CultistBossFireBall
            || entity.type == ProjectileID.CultistBossFireBallClone
            || entity.type == ProjectileID.DD2BetsyFireball
            || entity.type == ProjectileID.DD2BetsyFlameBreath;

    }

    public override void AI(Projectile projectile)
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            if (Main.rand.NextBool(20))
            {
                Point16 topLeftPoint = (projectile.TopLeft - new Vector2(32, 32)).ToTileCoordinates16();
                Point16 botRightPoint = (projectile.BottomRight + new Vector2(32, 32)).ToTileCoordinates16();

                for (int x = topLeftPoint.X; x < botRightPoint.X; x++)
                {
                    for (int y = topLeftPoint.Y; y < botRightPoint.Y; y++)
                    {
                        if (!WorldGen.InWorld(x, y)) continue;
                        Tile tile = Main.tile[x, y];
                        if (tile.HasTile && TileLists.FlammableTiles.Contains(tile.TileType))
                        {
                            if (tile.TileType == TileID.Grass)
                            {
                                tile.TileType = TileID.AshGrass; // Replace grass tiles because trees are weird
                                NetMessage.SendTileSquare(-1, x, y);
                                continue;
                            }
                            WorldGen.KillTile(x, y, noItem: true);

                            WorldGen.PlaceTile(x, y, TileID.Ash);

                            NetMessage.SendTileSquare(-1, x, y);
                        }
                    }
                }
            }
        }
        AITimer++;
    }
}
