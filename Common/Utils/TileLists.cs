using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TerrorMod.Common.Utils
{
    public class TileLists : TileID
    {
        public static HashSet<int> FlammableTiles { get; private set; } = new HashSet<int>()
        {
            /* Ground Tiles */ Grass, Plants,
            /* Wood */ WoodBlock, Ebonwood, Shadewood, PalmWood, RichMahogany, BorealWood, LeafBlock, DynastyWood, LivingWood,
            /* Furniture */ Saplings, Sunflower,
            /* Plants */ BloomingHerbs, ImmatureHerbs, MatureHerbs
        };
    }
}
