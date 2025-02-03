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
    public class RecipeRandomizer : ModSystem
    {
        string worldSeed => WorldGen.currentWorldSeed; // World seed is used to remove and add ingredients

        public override void OnWorldLoad()
        {
            foreach (Recipe recipe in Main.recipe)
            {
                int itemType = recipe.createItem.type;
                if (ItemLists.PreHM_Items.Contains(itemType))
                {
                    UnifiedRandom random = GetUnifiedRandomForRecipe(itemType);
                    int numOfIngredients = GetNewIngredientAmount(random);
                    List<int> itemIDs = GetRandomItemIDs(random, numOfIngredients);
                    for (int i = 0; i < numOfIngredients; i++)
                    {
                        int itemID = itemIDs[i];
                        int ingredientMaxStack = ContentSamples.ItemsByType[itemID].maxStack;
                        int stackCount = 1;
                        if (ingredientMaxStack > 1) stackCount = random.Next(8, 32);
                        recipe.AddIngredient(itemID, stackCount);
                    }
                }
            }
        }

        public override void OnWorldUnload()
        {
            foreach (Recipe recipe in Main.recipe)
            {
                if (ItemLists.PreHM_Items.Contains(recipe.createItem.type))
                {
                    UnifiedRandom random = GetUnifiedRandomForRecipe(recipe.createItem.type);
                    int numOfIngredients = GetNewIngredientAmount(random);

                    recipe.requiredItem.RemoveRange(recipe.requiredItem.Count - numOfIngredients, numOfIngredients);   
                }
            }
        }

        int GetSeedForRandom(int recipeItemID)
        {
            int seedRand = int.Parse(worldSeed.Substring(0, 7));  // Seed for random is comprised of the first 7 numbers of the world seed +
            seedRand += recipeItemID;                             // the id of the item whose recipe is being randomized
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
                int itemID = random.NextFromCollection(ItemLists.PreHM_Materials.Concat(ItemLists.PreHM_Items).ToList());

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
