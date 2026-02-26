using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Localization;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Buffs.Debuffs.Temperature;

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

    #region Temperature Info Display
    public bool ShowTemperature { get; set; } = false;
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
    #endregion

    public override void ResetEffects()
    {
        TargetTemperature = 0;
        LavaTemperature = DEFAULT_LAVA_TEMPERATURE;
        HeatTolerance = DEFAULT_HEAT_TOLERANCE;
        ColdTolerance = DEFAULT_COLD_TOLERANCE;
        SuperHeatTolerance = DEFAULT_SUPER_HEAT_TOLERANCE;
        SuperColdTolerance = DEFAULT_SUPER_COLD_TOLERANCE;
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

    #region Temperature Calculations and Checks

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
            if (Main.IsItDay())
            {
                biomeTemp += 20;
            }
            else
            {
                biomeTemp -= 20;
            }
        }

        if (Player.ZoneDesert)
        {
            if (Main.IsItDay())
            {
                biomeTemp += 50;
            }
            else
            {
                biomeTemp -= 40;
            }
        }

        if (Player.ZoneSnow)
        {
            if (Main.IsItDay())
            {
                biomeTemp -= 40;
            }
            else
            {
                biomeTemp -= 80;
            }
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
                waterTemp -= 25;
            }
            else
            {
                waterTemp += 0;
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
        else if (Player.adjLava)
        {
            TargetTemperature += LavaTemperature / 10;
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
    #endregion

    void LerpCurrentTemperature()
    {
        CurrentTemperature = MathHelper.Lerp(CurrentTemperature, TargetTemperature, TemperatureChangeRate * (1 / 100f));
        if (MathF.Abs(CurrentTemperature - TargetTemperature) <= 0.5f)
        {
            CurrentTemperature = TargetTemperature;
        }
    }

    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        CurrentTemperature = 0;
    }

    float vignetteIntensity = 0f;
    public override void PostUpdateMiscEffects()
    {
        if (!Main.dedServ)
        {
            if (CurrentTemperature > 0)
            {
                HeatShaderControl();
            }
            else
            {
                ColdShaderControl();
            }
        }

        if (CurrentTemperature >= 100)
        {
            Player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TerrorMod.Buffs.SuperHotDebuff.DeathMessage", Main.LocalPlayer.name)), 9999, 1);
        }
        else if (CurrentTemperature <= -100)
        {
            Player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TerrorMod.Buffs.SuperColdDebuff.DeathMessage", Main.LocalPlayer.name)), 9999, 1);
        }
    }

    public override void PreUpdateBuffs()
    {
        TemperatureBuffControl();
    }

    void TemperatureBuffControl()
    {
        if (CurrentTemperature <= SuperColdTolerance)
        {
            Player.AddBuff(BuffType<SuperColdDebuff>(), 2);
        }
        else if (CurrentTemperature <= ColdTolerance)
        {
            Player.AddBuff(BuffType<ColdDebuff>(), 2);
        }
        else if (CurrentTemperature >= SuperHeatTolerance)
        {
            Player.AddBuff(BuffType<SuperHotDebuff>(), 2);
        }
        else if (CurrentTemperature >= HeatTolerance)
        {
            Player.AddBuff(BuffType<HotDebuff>(), 2);
        }
    }

    void HeatShaderControl()
    {
        float intensity = MathHelper.Clamp((CurrentTemperature - HeatTolerance) / HeatTolerance, 0, 1);
        if (CurrentTemperature >= HeatTolerance)
        {
            vignetteIntensity = MathHelper.Lerp(vignetteIntensity, MathF.Max(intensity, 0.5f), 1 / 60f);
        }
        else
        {
            vignetteIntensity = MathHelper.Lerp(vignetteIntensity, 0, 1 / 60f);
        }

        if (!Filters.Scene["TerrorMod:WavyShader"].IsActive())
        {
            Filters.Scene.Activate("TerrorMod:WavyShader");
        }
        Filters.Scene["TerrorMod:WavyShader"].GetShader().UseIntensity(intensity);
        Filters.Scene["TerrorMod:WavyShader"].GetShader().UseProgress(intensity);

        if (!Filters.Scene["TerrorMod:VignetteShader"].IsActive())
        {
            Filters.Scene.Activate("TerrorMod:VignetteShader");
        }
        Filters.Scene["TerrorMod:VignetteShader"].GetShader().UseIntensity(vignetteIntensity * 1);
        Filters.Scene["TerrorMod:VignetteShader"].GetShader().UseProgress(vignetteIntensity * 1);
        Filters.Scene["TerrorMod:VignetteShader"].GetShader().UseColor(Color.Red);
    }

    void ColdShaderControl()
    {
        float absColdTolerance = MathF.Abs(ColdTolerance);
        float absCurrentTemperature = MathF.Abs(CurrentTemperature);
        float intensity = MathHelper.Clamp((absCurrentTemperature - absColdTolerance) / absColdTolerance, 0, 1);
        if (absCurrentTemperature >= absColdTolerance)
        {
            vignetteIntensity = MathHelper.Lerp(vignetteIntensity, MathF.Max(intensity, 0.5f), 1 / 60f);
        }
        else
        {
            vignetteIntensity = MathHelper.Lerp(vignetteIntensity, 0, 1 / 60f);
        }

        if (!Filters.Scene["TerrorMod:VignetteShader"].IsActive())
        {
            Filters.Scene.Activate("TerrorMod:VignetteShader");
        }
        Filters.Scene["TerrorMod:VignetteShader"].GetShader().UseIntensity(vignetteIntensity * 1);
        Filters.Scene["TerrorMod:VignetteShader"].GetShader().UseProgress(vignetteIntensity * 1);
        Filters.Scene["TerrorMod:VignetteShader"].GetShader().UseColor(Color.Blue);
    }
}