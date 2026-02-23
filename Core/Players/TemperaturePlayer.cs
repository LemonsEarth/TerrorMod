namespace TerrorMod.Core.Players;

public class TemperaturePlayer : ModPlayer
{
    public float CurrentTemperature { get; private set; } = 0;
    public float TargetTemperature { get; set; } = 0;

    public const float DEFAULT_TEMPERATURE_CHANGE_RATE = 1;
    public float TemperatureChangeRate { get; set; } = 1;

    public const float DEFAULT_LAVA_TEMPERATURE = 200;
    public float LavaTemperature { get; set; } = DEFAULT_LAVA_TEMPERATURE;

    public float WaterTemperature => CalculateWaterTemperature();

    public const float DEFAULT_HEAT_TOLERANCE = 30;
    public float HeatTolerance { get; set; } = DEFAULT_HEAT_TOLERANCE;

    public const float DEFAULT_SUPER_HEAT_TOLERANCE = 60;
    public float SuperHeatTolerance { get; set; } = DEFAULT_SUPER_HEAT_TOLERANCE;

    public const float DEFAULT_COLD_TOLERANCE = -30;
    public float ColdTolerance { get; set; } = DEFAULT_COLD_TOLERANCE;

    public const float DEFAULT_SUPER_COLD_TOLERANCE = -60;
    public float SuperColdTolerance { get; set; } = DEFAULT_SUPER_COLD_TOLERANCE;

    public bool ShowTemperature { get; set; } = false;

    public override void ResetEffects()
    {
        TargetTemperature = 0;
        LavaTemperature = DEFAULT_LAVA_TEMPERATURE;
        HeatTolerance = DEFAULT_HEAT_TOLERANCE;
        ColdTolerance = DEFAULT_COLD_TOLERANCE;
        SuperHeatTolerance = DEFAULT_SUPER_HEAT_TOLERANCE;
        SuperColdTolerance = DEFAULT_SUPER_COLD_TOLERANCE;
    }

    public override void ResetInfoAccessories()
    {
        ShowTemperature = false;
    }

    public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer)
    {
        if (otherPlayer.TemperaturePlayer().ShowTemperature)
        {
            ShowTemperature = true;
        }
    }

    public override void PostUpdate()
    {
        CheckAndAddEquipmentTemperature();
        CheckAndAddBuffTemerature();
        CheckAndAddBiomeTemerature();
        CheckAndAddWaterTemperature();
        CheckAndAddLavaTemperature();
        LerpCurrentTemperature();
    }

    float CalculateFeverBuffTemperatures()
    {
        int buffTemp = 0;
        if (Player.HasBuff(BuffID.Poisoned) || Player.HasBuff(BuffID.PotionSickness) || Player.HasBuff(BuffID.ManaSickness) || Player.HasBuff(BuffID.Rabies))
        {
            buffTemp += 15;
        }
        return buffTemp;
    }

    float CalculateFireBuffTemperatures()
    {
        int buffTemp = 0;
        if (Player.HasBuff(BuffID.OnFire) || Player.HasBuff(BuffID.Burning))
        {
            buffTemp += 30;
        }

        if (Player.HasBuff(BuffID.OnFire3) || Player.HasBuff(BuffID.CursedInferno) || Player.HasBuff(BuffID.ShadowFlame))
        {
            buffTemp += 40;
        }

        if (Player.HasBuff(BuffID.Ichor))
        {
            buffTemp += 15;
        }

        if (Player.HasBuff(BuffID.Frostburn))
        {
            buffTemp -= 30;
        }

        if (Player.HasBuff(BuffID.Frostburn2))
        {
            buffTemp -= 40;
        }

        return buffTemp;
    }

    float CalculateBuffTemperatures()
    {
        float buffTemp = 0;
        if (Player.HasBuff(BuffID.ObsidianSkin))
        {
            buffTemp -= 15;
        }

        if (Player.HasBuff(BuffID.Ironskin))
        {
            buffTemp += 10;
        }

        if (Player.HasBuff(BuffID.Shine))
        {
            buffTemp += 15;
        }

        if (Player.HasBuff(BuffID.Campfire))
        {
            buffTemp += 20;
        }

        if (Player.HasBuff(BuffID.Warmth))
        {
            buffTemp += 25;
        }

        buffTemp += CalculateFireBuffTemperatures();
        buffTemp += CalculateFeverBuffTemperatures();

        return buffTemp;
    }

    void CheckAndAddBuffTemerature()
    {
        TargetTemperature += CalculateBuffTemperatures();
    }

    float CalculateBiomeTemperatures()
    {
        float biomeTemp = 0;
        if (Player.ZoneUnderworldHeight)
        {
            biomeTemp += 60;
        }

        if (Player.ZoneBeach)
        {
            biomeTemp += 20;
        }

        if (Player.ZoneDesert)
        {
            biomeTemp += 40;
        }

        if (Player.ZoneSnow)
        {
            biomeTemp -= 40;
        }

        if (Player.ZoneDungeon)
        {
            biomeTemp -= 20;
        }

        if (Player.ZoneMeteor)
        {
            biomeTemp += 40;
        }

        return biomeTemp;
    }

    void CheckAndAddBiomeTemerature()
    {
        TargetTemperature += CalculateBiomeTemperatures();
    }

    float CalculateWaterTemperature()
    {
        float waterTemp = 0;

        if (Player.ZoneForest)
        {
            waterTemp -= 10;
        }

        if (Player.ZoneDesert)
        {
            if (Main.IsItDay()) // colder during the day, warmer during the night
            {
                waterTemp -= 30;
            }
            else
            {
                waterTemp += 20;
            }
        }

        if (Player.ZoneBeach)
        {
            waterTemp -= 20;
        }

        if (Player.ZoneSnow)
        {
            if (Main.IsItDay())
            {
                waterTemp -= 20;
            }
            else
            {
                waterTemp -= 50;
            }
        }

        if (Player.ZoneUnderworldHeight)
        {
            waterTemp -= 15;
        }

        return waterTemp;
    }

    void CheckAndAddWaterTemperature()
    {
        if (Player.wet || Player.HasBuff(BuffID.Wet))
        {
            TargetTemperature += WaterTemperature;
        }
    }

    void CheckAndAddLavaTemperature()
    {
        if (Player.lavaWet)
        {
            TargetTemperature += LavaTemperature;
        }
    }

    float CheckArmorSetTemperature(int headType, int bodyType, int legsType, float tempPerPiece)
    {
        float armorTemp = 0;
        if (Player.CheckHead(headType))
        {
            armorTemp += tempPerPiece;
        }

        if (Player.CheckBody(bodyType))
        {
            armorTemp += tempPerPiece;
        }

        if (Player.CheckLegs(legsType))
        {
            armorTemp += tempPerPiece;
        }

        return armorTemp;
    }

    float CalculateEquipmentTemperature()
    {
        float equipTemp = 0;

        equipTemp += CheckArmorSetTemperature(ItemID.MoltenHelmet, ItemID.MoltenBreastplate, ItemID.MoltenGreaves, 20);
        equipTemp += CheckArmorSetTemperature(ItemID.MeteorHelmet, ItemID.MeteorSuit, ItemID.MeteorLeggings, 15);
        equipTemp += CheckArmorSetTemperature(ItemID.SolarFlareHelmet, ItemID.SolarFlareBreastplate, ItemID.SolarFlareLeggings, 30);
        equipTemp -= CheckArmorSetTemperature(ItemID.FrostHelmet, ItemID.FrostBreastplate, ItemID.FrostLeggings, 15);
        equipTemp += CheckArmorSetTemperature(ItemID.EskimoHood, ItemID.EskimoCoat, ItemID.EskimoPants, 10);
        equipTemp += CheckArmorSetTemperature(ItemID.PinkEskimoHood, ItemID.PinkEskimoCoat, ItemID.PinkEskimoPants, 10);

        if (Player.CheckBody(ItemID.FlinxFurCoat))
        {
            equipTemp += 15;
        }

        return equipTemp;
    }

    void CheckAndAddEquipmentTemperature()
    {
        TargetTemperature += CalculateEquipmentTemperature();
    }

    void LerpCurrentTemperature()
    {
        CurrentTemperature = MathHelper.Lerp(CurrentTemperature, TargetTemperature, TemperatureChangeRate * (1 / 120f));
    }
}