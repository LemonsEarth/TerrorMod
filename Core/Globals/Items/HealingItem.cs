using System.Collections.Generic;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Core.Globals.Items;

public class HealingItem : GlobalItem
{
    public override bool InstancePerEntity => true;
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.healLife > 0;
    }

    public override void SetDefaults(Item item)
    {
        item.useTime = 120;
        item.useAnimation = 120;
    }

    public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
    {

    }

    public override bool? UseItem(Item item, Player player)
    {
        return null;
    }

    public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
    {
        
    }

    public override bool CanUseItem(Item item, Player player)
    {
        return !player.controlQuickHeal;
    }
}

public class HealingItemPlayer : ModPlayer
{
    public override void ResetEffects()
    {
        
    }

    public override void UpdateEquips()
    {
        
    }

    public override void PostUpdateMiscEffects()
    {
        
    }

    public override void PostUpdate()
    {

    }

    public override void PreUpdateMovement()
    {
        if (Player.HeldItem.healLife > 0 && Player.ItemAnimationActive)
        {
            Player.velocity.X *= 0.9f;
        }
    }
}
