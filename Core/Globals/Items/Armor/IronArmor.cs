namespace TerrorMod.Core.Globals.Items.Armor;

public class IronArmor : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.IronHelmet;
    }

    public override string IsArmorSet(Item head, Item body, Item legs)
    {
        if (head.type == ItemID.IronHelmet && body.type == ItemID.IronChainmail && legs.type == ItemID.IronGreaves)
        {
            return "IronIronskin";
        }
        return string.Empty;
    }

    public override void UpdateArmorSet(Player player, string set)
    {
        if (set == "IronIronskin") player.AddBuff(BuffID.Ironskin, 2);
    }
}
