using TerrorMod.Content.Buffs.Buffs;

namespace TerrorMod.Core.Globals.Items.Armor;

public class TungstenArmor : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.TungstenHelmet;
    }

    public override string IsArmorSet(Item head, Item body, Item legs)
    {
        if (head.type == ItemID.TungstenHelmet && body.type == ItemID.TungstenChainmail && legs.type == ItemID.TungstenGreaves)
        {
            return "TungstenPenetration";
        }
        return string.Empty;
    }

    public override void UpdateArmorSet(Player player, string set)
    {
        if (set == "TungstenPenetration") player.AddBuff(BuffType<TungstenPenetration>(), 2);
    }
}

public class TunstenPlayer : ModPlayer
{
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (Player.HasBuff(BuffType<TungstenPenetration>()))
        {
            modifiers.ArmorPenetration += 5;
        }
    }
}
