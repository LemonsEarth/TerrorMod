using System.Collections.Generic;

namespace TerrorMod.Core.Globals.Items.Armor;

public class MythrilArmor : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.JungleHat;
    }

    public override void UpdateEquip(Item item, Player player)
    {
        player.endurance += 5f / 100f;
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        var line = new TooltipLine(Mod, "Terror:MythrilDR", "Increases damage reduction by 5%");
        tooltips.Add(line);
    }
}
