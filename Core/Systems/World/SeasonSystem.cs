using System.IO;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Players;

namespace TerrorMod.Core.Systems.World;

public class SeasonSystem : ModSystem
{
    public static int CycleLength => 40;
    public static int DayCount => DayCountSystem.DayCount % CycleLength;

    public enum Season
    {
        EarlyAutumn,
        LateAutumn,
        EarlyWinter,
        LateWinter,
        EarlySpring,
        LateSpring,
        EarlySummer,
        LateSummer
    }

    public static Season CurrentSeason { get; set; } = Season.EarlyWinter;

    public override void PreUpdateWorld()
    {
        SetCurrentSeason();
        SeasonUpdate();
    }

    public override void PostUpdatePlayers()
    {

    }

    public static void SetCurrentSeason()
    {
        switch (DayCount)
        {
            case >= 0 and < 5:
                CurrentSeason = Season.EarlyAutumn;
                break;
            case >= 5 and < 10:
                CurrentSeason = Season.LateAutumn;
                break;
            case >= 10 and < 15:
                CurrentSeason = Season.EarlyWinter;
                break;
            case >= 15 and < 20:
                CurrentSeason = Season.LateWinter;
                break;
            case >= 20 and < 25:
                CurrentSeason = Season.EarlySpring;
                break;
            case >= 25 and < 30:
                CurrentSeason = Season.LateSpring;
                break;
            case >= 30 and < 35:
                CurrentSeason = Season.EarlySummer;
                break;
            case >= 35 and < 40:
                CurrentSeason = Season.LateSummer;
                break;
        }
    }

    public static void SeasonUpdate()
    {
        switch (CurrentSeason)
        {
            case Season.EarlyAutumn:
                EarlyAutumnUpdate();
                break;
            case Season.LateAutumn:
                LateAutumnUpdate();
                break;
            case Season.EarlyWinter:
                EarlyWinterUpdate();
                break;
            case Season.LateWinter:
                LateWinterUpdate();
                break;
            case Season.EarlySpring:
                EarlySpringUpdate();
                break;
            case Season.LateSpring:
                LateSpringUpdate();
                break;
            case Season.EarlySummer:
                EarlySummerUpdate();
                break;
            case Season.LateSummer:
                LateSummerUpdate();
                break;
        }
    }

    public const float EarlyAutumnTemperature = -5;
    public static void EarlyAutumnUpdate()
    {

    }

    public const float LateAutumnTemperature = -15;
    public static void LateAutumnUpdate()
    {
        // It rain forever
        Main.StartRain();
        Main.windSpeedTarget *= 1.02f;

        // Occasionally, water forms
        if (Main.rand.NextBool(30))
        {
            int randX = Main.rand.Next(0, Main.maxTilesX);
            int randY = Main.rand.Next(0, (int)Main.worldSurface);
            Tile tile = Main.tile[randX, randY];
            if (tile.HasTile && !Main.tile[randX, randY - 1].HasTile)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        WorldGen.PlaceLiquid(randX + i, randY + j, (byte)LiquidID.Water, byte.MaxValue);
                    }
                }
            }
        }
    }

    public const float EarlyWinterTemperature = -30;
    public static void EarlyWinterUpdate()
    {
        if (Main.rand.NextBool((10 / LemonUtils.GetWorldSize()) + 1))
        {
            Vector2 randPos = new Vector2(Main.rand.NextFloat(0, Main.maxTilesX * 16), Main.rand.NextFloat(0, 100 * 16));
            Projectile.NewProjectileDirect(
                new EntitySource_Misc("EarlyWinterSnowball"),
                randPos,
                Vector2.Zero,
                ProjectileType<FallingSnowball>(),
                10,
                5f
                );
        }
    }

    public const float LateWinterTemperature = -45;
    public static void LateWinterUpdate()
    {
        if (Main.rand.NextBool((5 / LemonUtils.GetWorldSize()) + 1))
        {
            Vector2 randPos = new Vector2(Main.rand.Next(0, Main.maxTilesX * 16), Main.rand.Next(0, 100 * 16));
            Projectile.NewProjectileDirect(
                new EntitySource_Misc("EarlyWinterSnowball"),
                randPos,
                Vector2.Zero,
                ProjectileType<FallingSnowball>(),
                10,
                5f
                );

            // Freeze water
            for (int i = 0; i < 5; i++)
            {
                int randX = Main.rand.Next(0, Main.maxTilesX);
                int randY = Main.rand.Next(0, (int)Main.worldSurface);
                Tile tile = Main.tile[randX, randY];
                Tile tileAbove = Main.tile[randX, randY - 1];
                if (!tile.HasTile && !tileAbove.HasTile 
                    && tile.LiquidType == LiquidID.Water && tile.LiquidAmount > byte.MaxValue * 0.5f
                    && tileAbove.LiquidAmount == 0)
                {
                    WorldGen.PlaceTile(randX, randY, TileID.IceBlock, false);
                }
            }
        }
    }

    public const float EarlySpringTemperature = 5;
    public static void EarlySpringUpdate()
    {
        // Melting random snow blocks
        for (int i = 0; i < 1; i++)
        {
            int randX = Main.rand.Next(0, Main.maxTilesX);
            int randY = Main.rand.Next(0, (int)Main.worldSurface);
            Tile tile = Main.tile[randX, randY];
            if (tile.HasTile && tile.TileType == TileID.SnowBlock)
            {
                if (!Main.tile[randX, randY - 1].HasTile || !Main.tile[randX, randY + 1].HasTile || !Main.tile[randX - 1, randY].HasTile || !Main.tile[randX + 4, randY].HasTile)
                {
                    WorldGen.KillTile(randX, randY, noItem: true);
                }
            }
        }
    }

    public const float LateSpringTemperature = 15;
    public static void LateSpringUpdate()
    {
        // Melting random snow blocks
        for (int i = 0; i < 5; i++)
        {
            int randX = Main.rand.Next(0, Main.maxTilesX);
            int randY = Main.rand.Next(0, (int)Main.worldSurface);
            Tile tile = Main.tile[randX, randY];
            if (tile.HasTile && tile.TileType == TileID.SnowBlock)
            {
                if (!Main.tile[randX, randY - 1].HasTile || !Main.tile[randX, randY + 1].HasTile || !Main.tile[randX - 1, randY].HasTile || !Main.tile[randX + 4, randY].HasTile)
                {
                    WorldGen.KillTile(randX, randY, noItem: true);
                }
            }
        }
    }

    public const float EarlySummerTemperature = 30;
    public static void EarlySummerUpdate()
    {
        // Melting random snow blocks AND ice blocks
        for (int i = 0; i < 10; i++)
        {
            int randX = Main.rand.Next(0, Main.maxTilesX);
            int randY = Main.rand.Next(0, (int)Main.worldSurface);
            Tile tile = Main.tile[randX, randY];
            if (tile.HasTile && tile.TileType == TileID.SnowBlock || tile.TileType == TileID.IceBlock)
            {
                if (!Main.tile[randX, randY - 1].HasTile || !Main.tile[randX, randY + 1].HasTile || !Main.tile[randX - 1, randY].HasTile || !Main.tile[randX + 4, randY].HasTile)
                {
                    WorldGen.KillTile(randX, randY, noItem: true);
                    if (tile.TileType == TileID.IceBlock)
                    {
                        WorldGen.PlaceLiquid(randX, randY, (byte)LiquidID.Water, byte.MaxValue);
                    }
                }
            }
        }
    }

    public const float LateSummerTemperature = 45;
    public static void LateSummerUpdate()
    {
        // Melting random snow blocks AND ice blocks
        for (int i = 0; i < 30; i++)
        {
            int randX = Main.rand.Next(0, Main.maxTilesX);
            int randY = Main.rand.Next(0, (int)Main.worldSurface);
            Tile tile = Main.tile[randX, randY];
            if (tile.HasTile && tile.TileType == TileID.SnowBlock || tile.TileType == TileID.IceBlock)
            {
                if (!Main.tile[randX, randY - 1].HasTile || !Main.tile[randX, randY + 1].HasTile || !Main.tile[randX - 1, randY].HasTile || !Main.tile[randX + 4, randY].HasTile)
                {
                    WorldGen.KillTile(randX, randY, noItem: true);
                    if (tile.TileType == TileID.IceBlock)
                    {
                        WorldGen.PlaceLiquid(randX, randY, (byte)LiquidID.Water, byte.MaxValue);
                    }
                }
            }
        }

    }
}
