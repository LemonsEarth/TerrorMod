namespace TerrorMod.Core.Globals.Items.Armor;

public class SnowArmor : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.EskimoHood;
    }

    public override string IsArmorSet(Item head, Item body, Item legs)
    {
        if (head.type == ItemID.EskimoHood && body.type == ItemID.EskimoCoat && legs.type == ItemID.EskimoPants)
        {
            return "Eskimo";
        }
        return string.Empty;
    }

    public override void UpdateArmorSet(Player player, string set)
    {
        if (set == "Eskimo") player.AddBuff(BuffID.Warmth, 2);
    }
}
