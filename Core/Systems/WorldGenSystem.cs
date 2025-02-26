using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Tiles.Blocks;
using TerrorMod.Content.Tiles.Furniture;

namespace TerrorMod.Core.Systems
{
    public class WorldGenSystem : ModSystem
    {
        void ReplaceChests()
        {
            for (int i = 0; i < Main.maxChests; i++)
            {
                Chest chest = Main.chest[i];
                if (chest == null)
                {
                    continue;
                }

                int x = chest.x;
                int y = chest.y;

                Tile chestTile = Main.tile[x, y];
                if (chestTile.TileType == TileID.Containers)
                {
                    if (chestTile.TileFrameX == 1 * 36)
                    {
                        ReplaceChest(chest, x, y, i, ModContent.TileType<SimpleGoldenChest>());
                    }
                    else if (chestTile.TileFrameX == 11 * 36)
                    {
                        ReplaceChest(chest, x, y, i, ModContent.TileType<SimpleIceChest>());
                    }
                }
            }
        }

        void ReplaceChest(Chest originalChest, int originalX, int originalY, int originalIndex, int replaceType, int style = 1)
        {
            Item[] items = originalChest.item;
            Chest.DestroyChestDirect(originalX, originalY, originalIndex); // Seems to only delete the chest items and make the chest unusable?
            WorldGen.KillTile(originalX, originalY); // Actually destroys the chest

            int index = WorldGen.PlaceChest(originalX, originalY + 1, (ushort)replaceType, style: style);
            if (index == -1)
            {
                Mod.Logger.Warn("A chest wasn't properly replaced");
                return;
            }
            Main.chest[index].item = items;
        }

        void PlaceDungeonPactTiles()
        {
            int count = LemonUtils.GetWorldSize() * 8; // Starting at the maximum amount of pact tiles that can be placed

            while (count > 0)
            {
                int i = WorldGen.genRand.Next(0, Main.maxTilesX);
                int j = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 200);
                Tile tile = Main.tile[i, j];
                if (tile == null || !tile.HasTile)
                {
                    continue;
                }

                if (tile.TileType == TileID.BlueDungeonBrick || tile.TileType == TileID.PinkDungeonBrick || tile.TileType == TileID.GreenDungeonBrick)
                {
                    if (WorldGen.InWorld(i, j, 5) && !Main.tile[i, j - 1].HasTile)
                    {
                        WorldGen.PlaceTile(i, j - 1, ModContent.TileType<DungeonPactTile>());
                        count--;
                    }
                }
            }           

        }

        public override void PostWorldGen()
        {
            ReplaceChests();
            PlaceDungeonPactTiles();
        }
    }
}
