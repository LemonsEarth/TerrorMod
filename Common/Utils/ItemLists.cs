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
    public class ItemLists : ItemID
    {
        public static HashSet<int> PreHM_Items { get; private set; } = new HashSet<int>()
        {
            /* Pickaxes */ CactusPickaxe, CopperPickaxe, IronPickaxe, TinPickaxe, LeadPickaxe, SilverPickaxe, TungstenPickaxe, GoldPickaxe, PlatinumPickaxe, BonePickaxe, ReaverShark, FossilPickaxe, NightmarePickaxe, DeathbringerPickaxe, MoltenPickaxe,
            /* Axes */ CopperAxe, IronAxe, TinAxe, GoldAxe, PlatinumAxe, LeadAxe, SilverAxe, TungstenAxe, SawtoothShark, WarAxeoftheNight, BloodLustCluster, LucyTheAxe,
            /* Hammers */ WoodenHammer, CopperHammer, IronHammer, TinHammer, GoldHammer, PlatinumHammer, LeadHammer, SilverHammer, TungstenHammer, FleshGrinder, TheBreaker, EbonwoodHammer, ShadewoodHammer, AshWoodHammer, MoltenHamaxe, MeteorHamaxe, Rockfish,
            /* Hooks */ GrapplingHook, AmberHook, AmethystHook, TopazHook, DiamondHook, EmeraldHook, SapphireHook, WebSlinger, RubyHook, IvyWhip,
            /* Misc Tools */ GravediggerShovel, Umbrella, TragicUmbrella, IceMirror, MagicMirror, Sickle, StaffofRegrowth, EmptyBucket, LavaBucket, WaterBucket, HoneyBucket, BreathingReed, BugNet, FireproofBugNet, Binoculars, ChumBucket, FlareGun, DirtRod, BoneWand, LivingWoodWand, LeafWand, LivingMahoganyLeafWand, LivingMahoganyWand, HiveWand, MysticCoilSnake,
        

            /* Swords */ Swordfish, CopperShortsword, CopperBroadsword, WoodenSword, GoldBroadsword, GoldShortsword, PlatinumShortsword, PlatinumBroadsword, SilverBroadsword, SilverShortsword, TungstenBroadsword, TungstenShortsword, LeadShortsword, LeadBroadsword, IronShortsword, IronBroadsword, TinShortsword, TinBroadsword, CactusSword, EbonwoodSword, ShadewoodSword, ZombieArm, Ruler, AshWoodSword, AntlionClaw, BoneSword, Gladius, IceBlade, BatBat, TentacleSpike, LightsBane, BloodButcherer, BladeofGrass, Muramasa, Starfury, EnchantedSword, BluePhaseblade, BeeKeeper, NightsEdge, 
            /* Spears */ Spear, Trident, TheRottedFork, ThunderSpear,
            /* Boomerangs */ WoodenBoomerang, EnchantedBoomerang, Trimarang, IceBoomerang, Flamarang, ThornChakram, Shroomerang,
            /* Yoyos */ WoodYoyo,

            /* Bows */  WoodenBow, SilverBow, TungstenBow, BloodRainBow, BeesKnees, ShadewoodBow, EbonwoodBow, GoldBow, PlatinumBow, CopperBow, TinBow, LeadBow, IronBow, TendonBow, DemonBow,
            /* Guns */ FlintlockPistol, Minishark, Handgun, PhoenixBlaster, Boomstick, Musket, TheUndertaker, Revolver, QuadBarrelShotgun, PewMaticHorn, Sandgun, Blowpipe, Harpoon, SnowballCannon, StarCannon,
            /* Throwing */ Shuriken, ThrowingKnife, PoisonedKnife, Snowball, FrostDaggerfish, Javelin, BoneJavelin, BoneDagger, SpikyBall, MolotovCocktail, Grenade, StickyGrenade, BouncyGrenade, Bomb, StickyBomb, Dynamite, StickyDynamite,

            /* Wands */ WandofSparking, WandofFrosting, ThunderStaff, AmberStaff, AmethystStaff, TopazStaff, SapphireStaff, EmeraldStaff, RubyStaff, DiamondStaff, MagicMissile, Vilethorn, CrimsonRod, WeatherPain,
            /* Magic Guns */ ZapinatorGray, SpaceGun, BeeGun,
            /* Tomes */ WaterBolt, DemonScythe, BookofSkulls,

            /* Minions */ BabyBirdStaff, SlimeStaff, FlinxStaff, ImpStaff, HornetStaff, VampireFrogStaff, AbigailsFlower,
            /* Whips */ BlandWhip, BoneWhip, ThornWhip,


            /* Movement */ SpectreBoots, LightningBoots, FrostsparkBoots, TerrasparkBoots, LavaWaders, ObsidianWaterWalkingBoots, AmphibianBoots, FrogGear, FrogFlipper, BalloonPufferfish, WhiteHorseshoeBalloon, YellowHorseshoeBalloon, FartInABalloon, BlueHorseshoeBalloon, BundleofBalloons, BlizzardinaBalloon, SandstorminaBalloon, CloudinaBalloon, ArcticDivingGear, JellyfishDivingGear, TigerClimbingGear, FairyBoots, Magiluminescence, MoltenCharm, ObsidianSkull, ObsidianHorseshoe, Aglet, AnkletoftheWind, IceSkates,
            /* Combat */ FeralClaws, BandofStarpower, CelestialMagnet, CelestialCuffs, MagicCuffs, MagnetFlower, ManaFlower, BeeCloak, ObsidianShield, WhiteString,


            /* Armor */ CopperHelmet, CopperChainmail, CopperGreaves, TinHelmet, TinChainmail, TinGreaves, LeadHelmet, LeadAnvil, LeadGreaves, SilverHelmet, SilverChainmail, SilverGreaves, TungstenHelmet, TungstenChainmail, TungstenGreaves, GoldHelmet, GoldChainmail, GoldGreaves, PlatinumHelmet, PlatinumChainmail, PlatinumGreaves, IronHelmet, IronChainmail, IronGreaves, FossilHelm, FossilPants, FossilShirt, 
                        ShadowHelmet, ShadowGreaves, ShadowScalemail, CrimsonHelmet, CrimsonGreaves, CrimsonScalemail, MoltenHelmet, MoltenGreaves, MoltenBreastplate, Robe, FlinxFurCoat, NecroBreastplate, NecroGreaves, NecroHelmet, BeeHeadgear, BeeBreastplate, BeeGreaves, ObsidianHelm, ObsidianPants, ObsidianShirt, MeteorHelmet, MeteorLeggings, MeteorSuit, JunglePants, JungleShirt, JungleHat, CactusHelmet, CactusLeggings, CactusBreastplate,


            /* Crafting Stations */ Furnace, WorkBench, IronAnvil, LeadAnvil, Loom, Sawmill, Bottle, HeavyWorkBench, Keg,
            /* Tiles */ WoodenChair, WoodenDoor, WoodenTable, Campfire, HeartLantern,
            /* Misc */ Torch, WoodenArrow, MagicMirror, 

            /* Potions */ IronskinPotion, SwiftnessPotion, RegenerationPotion, ThornsPotion, SpelunkerPotion, MiningPotion, HunterPotion

        };

        public static HashSet<int> PreHM_Materials { get; private set; } = new HashSet<int>()
        {
            /* Materials */ AntlionMandible, Lens, Stinger, Vine, JungleSpores, Silk, Cobweb, Bone, BeeWax, ShadowScale, TissueSample, Vertebrae, RottenChunk, Feather, Gel, FlinxFur, Leather,
            /* Bars */ IronBar, TinBar, GoldBar, PlatinumBar, CrimtaneBar, MeteoriteBar, HellstoneBar, LeadBar, SilverBar, TungstenBar, CopperBar, DemoniteBar,
            /* Ores */ IronOre, TinOre, GoldOre, PlatinumOre, CrimtaneOre, Meteorite, Hellstone, LeadOre, SilverOre, TungstenOre, CopperOre, Obsidian, DemoniteOre,
            /* Gems */ Ruby, Sapphire, Topaz, Amber, Diamond, Emerald, Amethyst,
            /* Herbs */ Daybloom, Waterleaf, Blinkroot, Moonglow, Deathweed, Shiverthorn, Fireblossom,
            /* Seeds */ DaybloomSeeds, WaterleafSeeds, BlinkrootSeeds, MoonglowSeeds, DeathweedSeeds, ShiverthornSeeds, FireblossomSeeds,
            /* Blocks */ Wood, StoneBlock, GrayBrick, Snowball, SnowBlock, IceBlock, SandBlock, HardenedSand, DesertFossil, FossilOre, DirtBlock, Glass,
        };
    }
}
