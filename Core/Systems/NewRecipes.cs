using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TerrorMod.Common.Utils;

namespace TerrorMod.Core.Systems
{
    public class NewRecipes : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe eskimoHood = Recipe.Create(ItemID.EskimoHood);
            eskimoHood.AddIngredient(ItemID.Silk, 4);
            eskimoHood.AddTile(TileID.Loom);
            eskimoHood.Register();

            Recipe eskimoCoat = Recipe.Create(ItemID.EskimoCoat);
            eskimoCoat.AddIngredient(ItemID.Silk, 6);
            eskimoCoat.AddTile(TileID.Loom);
            eskimoCoat.Register();

            Recipe eskimoPants = Recipe.Create(ItemID.EskimoPants);
            eskimoPants.AddIngredient(ItemID.Silk, 3);
            eskimoPants.AddTile(TileID.Loom);
            eskimoPants.Register();
        }
    }
}
