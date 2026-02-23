namespace TerrorMod.Core.Globals.Items.Armor;

public class LeadArmor : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.LeadHelmet;
    }

    public override string IsArmorSet(Item head, Item body, Item legs)
    {
        if (head.type == ItemID.LeadHelmet && body.type == ItemID.LeadChainmail && legs.type == ItemID.LeadGreaves)
        {
            return "LeadSkin";
        }
        return string.Empty;
    }

    public override void UpdateArmorSet(Player player, string set)
    {
        if (set == "LeadSkin") player.GetModPlayer<LeadPlayer>().leadArmorSet = true;
    }
}

public class LeadPlayer : ModPlayer
{
    public bool leadArmorSet = false;

    public override void ResetEffects()
    {
        leadArmorSet = false;
    }
}
