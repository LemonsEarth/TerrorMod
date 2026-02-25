using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.Items;

public class GunItem : GlobalItem
{
    public override bool InstancePerEntity => true;
    int baseJamChanceDenominator = 16;
    int jamChanceDenominator = 16;
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.useAmmo == AmmoID.Bullet;
    }

    public override bool? UseItem(Item item, Player player)
    {

        return null;
    }

    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (Main.rand.NextBool(jamChanceDenominator))
        {
            SoundEngine.PlaySound(SoundID.Item11 with { PitchRange = (-0.5f, -0.4f), Volume = 0.8f }, player.Center);
            SoundEngine.PlaySound(SoundID.Item14 with { PitchRange = (0.6f, 0.8f), Volume = 0.4f }, player.Center);
            if (SoundEngine.FindActiveSound(item.UseSound.Value) != null)
            {
                SoundEngine.FindActiveSound(item.UseSound.Value).Stop();
            }
            jamChanceDenominator = baseJamChanceDenominator;
            return false;
        }
        if (jamChanceDenominator >= 3) jamChanceDenominator -= 2;
        return true;
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {

    }
}
