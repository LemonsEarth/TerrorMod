using System.Linq;

namespace TerrorMod.Core.Globals.Items;

public class RodOfHarmony : GlobalItem
{
    public override bool InstancePerEntity => true;
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.RodOfHarmony;
    }

    public override bool? UseItem(Item item, Player player)
    {
        if (Main.npc.Any(npc => npc.active && npc.boss))
        {
            player.AddBuff(BuffID.ChaosState, 300);
        }
        return null;
    }
}
