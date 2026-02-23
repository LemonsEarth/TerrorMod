using System.Collections.Generic;

namespace TerrorMod.Core.Players;

public class FishingPlayer : ModPlayer
{
    public int fishingPower = 0;

    public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
    {
        if (Player.anglerQuestsFinished == 3)
        {
            Item item = new Item();
            rewardItems.Add(item);
        }
    }

    public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
    {
        switch (fishingPower)
        {
            case 0:
                fishingLevel = 0;
                break;
            case 1:
                if (fishingLevel > 0.05f)
                {
                    fishingLevel = 0.05f;
                }
                break;
            case 2:
                if (fishingLevel > 0.15f)
                {
                    fishingLevel = 0.15f;
                }
                break;
            case 3:
                if (fishingLevel > 0.15f)
                {
                    fishingLevel = 0.15f;
                }
                break;
            case 4:
                if (fishingLevel > 0.30f)
                {
                    fishingLevel = 0.30f;
                }
                break;
            case 5:
                if (fishingLevel > 0.50f)
                {
                    fishingLevel = 0.50f;
                }
                break;
        }
    }
}