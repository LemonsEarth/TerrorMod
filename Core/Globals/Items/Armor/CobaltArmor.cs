using System.Collections.Generic;

namespace TerrorMod.Core.Globals.Items.Armor;

public class CobaltArmor : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.CobaltHat
            || entity.type == ItemID.CobaltHelmet
            || entity.type == ItemID.CobaltMask
            || entity.type == ItemID.CobaltBreastplate
            || entity.type == ItemID.CobaltLeggings;
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        string text = string.Empty;
        if (item.type == ItemID.CobaltHat
            || item.type == ItemID.CobaltHelmet
            || item.type == ItemID.CobaltMask)
        {
            text = "Chance to inflict Poisoned or Venom on hit";
        }
        else if (item.type == ItemID.CobaltBreastplate)
        {
            text = "Chance to inflict Shadowflame or Cursed Flames on hit";
        }
        else
        {
            text = "Chance to inflict Ichor or Oiled on hit";
        }
        var line = new TooltipLine(Mod, "Terror:CobaltDebuffs", text);
        tooltips.Add(line);
    }

    public override void UpdateEquip(Item item, Player player)
    {
        if (item.type == ItemID.CobaltHat
            || item.type == ItemID.CobaltHelmet
            || item.type == ItemID.CobaltMask)
        {
            player.GetModPlayer<CobaltPlayer>().cobaltHead = true;
        }

        if (item.type == ItemID.CobaltBreastplate)
        {
            player.GetModPlayer<CobaltPlayer>().cobaltBody = true;
        }

        if (item.type == ItemID.CobaltLeggings)
        {
            player.GetModPlayer<CobaltPlayer>().cobaltLegs = true;
        }
    }
}

public class CobaltPlayer : ModPlayer
{
    public bool cobaltHead = false;
    public bool cobaltBody = false;
    public bool cobaltLegs = false;

    public override void ResetEffects()
    {
        cobaltHead = false;
        cobaltBody = false;
        cobaltLegs = false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (cobaltHead)
        {
            if (Main.rand.NextBool(10)) target.AddBuff(BuffID.Poisoned, 300);
            if (Main.rand.NextBool(10)) target.AddBuff(BuffID.Venom, 300);
        }

        if (cobaltBody)
        {
            if (Main.rand.NextBool(10)) target.AddBuff(BuffID.ShadowFlame, 300);
            if (Main.rand.NextBool(10)) target.AddBuff(BuffID.CursedInferno, 300);
        }

        if (cobaltLegs)
        {
            if (Main.rand.NextBool(10)) target.AddBuff(BuffID.Ichor, 300);
            if (Main.rand.NextBool(10)) target.AddBuff(BuffID.Oiled, 300);
        }
    }
}
