using Terraria.Localization;
using static Terraria.ID.ItemID;

namespace TerrorMod.Common.Utils;

public class RecipeGroups : ModSystem
{
    public static RecipeGroup BossTrophies;
    public override void Unload()
    {
        BossTrophies = null;
    }

    public override void AddRecipeGroups()
    {
        BossTrophies = new RecipeGroup(
            () => $"{Language.GetTextValue("LegacyMisc.37")} Boss Trophy",
            KingSlimeTrophy, EyeofCthulhuTrophy, EaterofWorldsTrophy, BrainofCthulhuTrophy, QueenBeeTrophy, SkeletronTrophy, DeerclopsTrophy, WallofFleshTrophy, DestroyerTrophy, RetinazerTrophy, SpazmatismTrophy, SkeletronPrimeTrophy, PlanteraTrophy, GolemTrophy, MartianSaucerTrophy
            );
        RecipeGroup.RegisterGroup("TerrorMod:AnyBossTrophy", BossTrophies);
    }
}
