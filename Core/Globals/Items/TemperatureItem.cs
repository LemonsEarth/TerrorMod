using System.Collections.Generic;
using TerrorMod.Content.Buffs.Debuffs;
using static Terraria.ID.ItemID;

namespace TerrorMod.Core.Globals.Items;

public class TemperatureItem : GlobalItem
{
    public override bool InstancePerEntity => true;

    public static HashSet<int> ColdAccessories { get; private set; }
    public static HashSet<int> HotAccessories { get; private set; }

    public override void SetStaticDefaults()
    {
        ColdAccessories = new HashSet<int>()
        {
            BlizzardinaBottle,
            BlizzardinaBalloon,
            WhiteHorseshoeBalloon,
            BundleofBalloons,
            FrozenWings,
            GhostWings,
            FrozenTurtleShell,
            FrozenShield,
            ObsidianSkull,
            ObsidianShield,
            ObsidianHorseshoe,
        };

        HotAccessories = new HashSet<int>()
        {
            SunStone,
            CelestialStone,
            CelestialShell,
            FartinaJar,
            FartInABalloon,
            BalloonHorseshoeFart,
            LavaWaders,
            HellfireTreads,
            FlameWakerBoots,
            LavaCharm,
            Magiluminescence,
            MagmaStone,
            LavaSkull,
            MoltenCharm,
            Jetpack,
            FlameWings,
            WingsSolar,
            FireGauntlet,
            MoltenQuiver,
            HandWarmer,
            MoltenSkullRose,
            LavaproofTackleBag,
            LavaFishingHook,
        };
    }

    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.accessory;
    }

    public override void UpdateAccessory(Item item, Player player, bool hideVisual)
    {
        if (HotAccessories.Contains(item.type))
        {
            player.TemperaturePlayer().TargetTemperature += 5;
        }
        else if (ColdAccessories.Contains(item.type))
        {
            player.TemperaturePlayer().TargetTemperature -= 5;
        }
    }
}
