using System.Collections.Generic;
using System.Linq;
using Terraria.Utilities;
using TerrorMod.Common.Utils;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Systems;

public class RecipeRandomizer : ModSystem
{
    string playedID => Main.clientUUID; // Player ID makes each recipe unique and is kept the same every reload

    public override void PostAddRecipes()
    {
        if (!TerrorClientConfigs.clientConfig.EnableRecipeRandomizer) return;
        foreach (Recipe recipe in Main.recipe)
        {
            AddItemToProgressionList(recipe);

            RandomizePreHMRecipes(recipe);
            RandomizeEarlyHMRecipes(recipe);
        }
    }

    void AddItemToProgressionList(Recipe recipe)
    {
        Item item = recipe.createItem;
        if (ItemLists.ItemBlacklist.Contains(item.type)) return;
        if (item.damage <= 0 || ItemLists.PreHM_Items.Contains(item.type)) return;
        if (item.damage < 30 && item.rare <= ItemRarityID.Green)
        {
            ItemLists.PreHM_Items.Add(item.type); // Adding potentially missed and modded items to the list
        }
        else if (item.damage < 60 && item.rare <= ItemRarityID.LightPurple)
        {
            ItemLists.HM_Items.Add(item.type);
        }
    }

    void RandomizePreHMRecipes(Recipe recipe)
    {
        int itemType = recipe.createItem.type;
        if (ItemLists.PreHM_Items.Contains(itemType))
        {
            UnifiedRandom random = GetUnifiedRandomForRecipe(itemType);
            int numOfIngredients = GetNewIngredientAmount(random);
            List<int> itemIDs = GetRandomItemIDs(random, numOfIngredients, false);
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

    void RandomizeEarlyHMRecipes(Recipe recipe)
    {
        int itemType = recipe.createItem.type;
        if (ItemLists.HM_Items.Contains(itemType))
        {
            UnifiedRandom random = GetUnifiedRandomForRecipe(itemType);
            int numOfIngredients = GetNewIngredientAmount(random);
            List<int> itemIDs = GetRandomItemIDs(random, numOfIngredients, true);
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


    bool ItemIsValidIngredient(int resultID, int ingredientID)
    {
        bool valid = (resultID != ingredientID)
            && !Main.recipe.Any(rec => rec.createItem.type == ingredientID && rec.requiredTile.Contains(resultID));
        return valid;
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

    List<int> GetRandomItemIDs(UnifiedRandom random, int amount, bool hardmode)
    {
        List<int> itemIDs = new List<int>();
        List<int> collection = !hardmode ? ItemLists.PreHM_Materials.ToList() : ItemLists.HM_Materials.Union(ItemLists.PreHM_Materials).ToList();
        for (int i = 0; i < amount; i++)
        {
            int itemID = random.NextFromCollection(collection);

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
