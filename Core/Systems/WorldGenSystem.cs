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
using Terraria.Utilities;
using Terraria.WorldBuilding;
using TerrorMod.Common.Utils;
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
                if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 1 * 36)
                {
                    Item[] items = chest.item;
                    Chest.DestroyChestDirect(x, y, i); // Seems to only delete the chest items and make the chest unusable?
                    WorldGen.KillTile(x, y); // Actually destroys the chest

                    int index = WorldGen.PlaceChest(x, y + 1, (ushort)ModContent.TileType<SimpleGoldenChest>(), style: 1);
                    if (index == -1)
                    {
                        Mod.Logger.Warn("A chest wasn't properly replaced");
                        continue;
                    }
                    Main.chest[index].item = items;
                }       
            }
        }

        public override void PostWorldGen()
        {
            ReplaceChests();
        }
    }
}
