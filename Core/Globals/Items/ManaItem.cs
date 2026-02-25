using System.Collections.Generic;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Core.Globals.Items;

public class ManaItem : GlobalItem
{
    public override bool InstancePerEntity => true;
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.healMana > 0;
    }

    public override bool? UseItem(Item item, Player player)
    {
        player.AddBuff(BuffType<ManaMalady>(), 60 * 20);
        return null;
    }

    public override bool CanUseItem(Item item, Player player)
    {
        return !player.HasBuff(BuffType<ManaMalady>());
    }
}
