using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TerrorMod.Common.Utils;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Systems
{
    public class RecipeRandomizer : ModSystem
    {
        string playedID => Main.clientUUID; // Player ID makes each recipe unique and is kept the same every reload
        Recipe[] originalRecipes;
        public override void PostAddRecipes()
        {
            if (!TerrorClientConfigs.clientConfig.EnableRecipeRandomizer) return;
            foreach (Recipe recipe in Main.recipe)
            {
                int itemType = recipe.createItem.type;
                if (ContentSamples.ItemsByType[itemType].damage > 0 && ContentSamples.ItemsByType[itemType].damage < 30 && !ItemLists.PreHM_Items.Contains(itemType))
                {
                    ItemLists.PreHM_Items.Add(itemType);
                }
                if (ItemLists.PreHM_Items.Contains(itemType))
                {
                    UnifiedRandom random = GetUnifiedRandomForRecipe(itemType);
                    int numOfIngredients = GetNewIngredientAmount(random);
                    List<int> itemIDs = GetRandomItemIDs(random, numOfIngredients);
                    for (int i = 0; i < numOfIngredients; i++)
                    {
                        int itemID = itemIDs[i];
                        if (!ItemIsValidIngredient(itemType, itemID)) itemID = ItemID.GlowingMushroom; // Should prevent an item from becoming its own ingredient
                        int ingredientMaxStack = ContentSamples.ItemsByType[itemID].maxStack;
                        int stackCount = 1;
                        if (ingredientMaxStack > 1) stackCount = random.Next(4, 16);
                        recipe.AddIngredient(itemID, stackCount);
                    }
                }
            }
        }

        bool ItemIsValidIngredient(int resultID, int ingredientID)
        {;
            return (resultID != ingredientID) && !Main.recipe.Any(rec => rec.createItem.type == ingredientID && rec.requiredTile.Contains(resultID));
        }

        int GetSeedForRandom(int recipeItemID)
        {
            int seedRand = 148259367; // some random number idk lol
            if (Main.netMode != NetmodeID.Server)
            {
                IEnumerable<int> playerIDASCII_Collection = playedID.Select(character => (int)character); // convert to ASCII values so the ID is only numbers
                string playedIDASCII = string.Join("", playerIDASCII_Collection); // Join collection elements into string
                seedRand = int.Parse(playedIDASCII.Substring(0, 7));  // Seed for random is comprised of the first 7 numbers of the player ID +
            }
            
            seedRand += recipeItemID;                                 // the id of the item whose recipe is being randomized
            return seedRand;
        }

        UnifiedRandom GetUnifiedRandomForRecipe(int recipeItemID)
        {
            UnifiedRandom random = new UnifiedRandom(GetSeedForRandom(recipeItemID));
            return random;
        }

        List<int> GetRandomItemIDs(UnifiedRandom random, int amount)
        {
            List<int> itemIDs = new List<int>();
            for (int i = 0; i < amount; i++)
            {
                int itemID = random.NextFromCollection(ItemLists.PreHM_Materials.ToList());

                itemIDs.Add(itemID);
            }
            return itemIDs;
        }

        int GetNewIngredientAmount(UnifiedRandom random)
        {
            int ingredientAmount = random.Next(1, 5);
            return ingredientAmount;
        }
    }
}
