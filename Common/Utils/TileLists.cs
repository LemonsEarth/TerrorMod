using System.Collections.Generic;

namespace TerrorMod.Common.Utils;

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
