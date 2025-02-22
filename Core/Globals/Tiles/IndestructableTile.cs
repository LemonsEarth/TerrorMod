using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TerrorMod.Content.NPCs.Hostile.Forest;
using TerrorMod.Common.Utils;

namespace TerrorMod.Core.Globals.Tiles
{
    public class IndestructableTile : GlobalTile
    {
        public override bool CanExplode(int i, int j, int type)
        {
            return type != TileID.Obsidian;
        }
    }
}
