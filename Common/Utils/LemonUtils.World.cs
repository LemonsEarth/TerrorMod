using System.Collections.Generic;
using Terraria.DataStructures;

namespace TerrorMod.Common.Utils;

/// <summary>
/// Contains a lot of utillities and global usings
/// </summary>
public static partial class LemonUtils
{
    public static int UnderworldDepth => Main.maxTilesY - 200;

    public static Point GetMouseTile()
    {
        return Main.MouseWorld.ToTileCoordinates();
    }

    public static bool TileIsExposedToAir(int i, int j)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                if (!WorldGen.InWorld(i + x, j + y)) continue;
                if (!Main.tile[i + x, j + y].HasTile)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool HasTile(this Point point)
    {
        return Main.tile[point].HasTile;
    }

    public static bool HasSolidTile(this Point point)
    {
        Tile tile = Framing.GetTileSafely(point);
        return tile.HasTile && Main.tileSolid[tile.TileType];
    }

    public static bool IsDungeonBrick(int tileID)
    {
        return tileID == TileID.BlueDungeonBrick || tileID == TileID.GreenDungeonBrick || tileID == TileID.PinkDungeonBrick;
    }

    /// <summary>
    /// Generate structure loot by filling chests in the given structure area.
    /// </summary>
    /// <param name="structureX"></param>
    /// <param name="structureY"></param>
    /// <param name="structureDims"></param>
    /// <param name="allPossibleItems"></param>
    public static void GenerateStructureLoot(int structureX, int structureY, Point16 structureDims, List<List<(int, int)>> allPossibleItems)
    {
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];
            if (chest == null) continue;

            Rectangle structureRect = new Rectangle(structureX, structureY, structureDims.X, structureDims.Y);
            if (!structureRect.Contains(chest.x, chest.y))
            {
                continue;
            }

            for (int i = 0; i < allPossibleItems.Count; i++)
            {
                List<(int, int)> possibleItems = allPossibleItems[i];
                (int, int) chosenItem = Main.rand.NextFromCollection(possibleItems);
                chest.item[i].SetDefaults(chosenItem.Item1);
                chest.item[i].stack = chosenItem.Item2;
            }
        }
    }

    /// <summary>
    /// Generate structure loot by filling chests in the given structure area with randomized stack sizes.
    /// </summary>
    /// <param name="structureX"></param>
    /// <param name="structureY"></param>
    /// <param name="structureDims"></param>
    /// <param name="allPossibleItems"></param>
    /// <param name="randomizedStackOffsetMin"></param>
    /// <param name="randomizedStackOffsetMax"></param>
    public static void GenerateStructureLoot(int structureX, int structureY, Point16 structureDims, List<List<(int, int)>> allPossibleItems, float randomizedStackOffsetMin, float randomizedStackOffsetMax)
    {
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];
            if (chest == null) continue;

            Rectangle structureRect = new Rectangle(structureX, structureY, structureDims.X, structureDims.Y);
            if (!structureRect.Contains(chest.x, chest.y))
            {
                continue;
            }

            for (int i = 0; i < allPossibleItems.Count; i++)
            {
                List<(int, int)> possibleItems = allPossibleItems[i];
                (int, int) chosenItem = Main.rand.NextFromCollection(possibleItems);
                chest.item[i].SetDefaults(chosenItem.Item1);
                if (chosenItem.Item2 > 1)
                {
                    chest.item[i].stack = Main.rand.Next((int)(chosenItem.Item2 * randomizedStackOffsetMin), (int)(chosenItem.Item2 * randomizedStackOffsetMax));
                }
                else
                {
                    chest.item[i].stack = chosenItem.Item2;
                }
            }
        }
    }

    /// <summary>
    /// Used for naturally generating an item in a vanilla chest. Use the wiki to find the appropriate chest's chestTileFrameX.
    /// If decreaseChanceDenominatorOnFail is true, the chance of the item generating will increase every unsuccessful attempt
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="chestTileFrameX"></param>
    /// <param name="chanceDenominator"></param>
    /// <param name="decreaseChanceDenominatorOnFail"></param>
    public static void GenerateItemInChest(int itemType, int chestTileFrameX, int chanceDenominator, bool decreaseChanceDenominatorOnFail = false)
    {
        int origChanceDenominator = chanceDenominator;
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];
            if (chest == null)
            {
                continue;
            }
            Tile chestTile = Main.tile[chest.x, chest.y];
            if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == chestTileFrameX * 36)
            {
                if (WorldGen.genRand.NextBool(chanceDenominator))
                {
                    for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(itemType);
                            break;
                        }
                    }
                }
                if (decreaseChanceDenominatorOnFail)
                {
                    chanceDenominator--;
                }
            }
        }
    }
}
