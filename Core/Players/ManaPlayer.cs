using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Common.Utils;
using System.Linq;

namespace TerrorMod.Core.Players;

public class ManaPlayer : ModPlayer
{
    float baseMagicDamageBoost = 20f;
    public override void PostUpdateEquips()
    {
        if (!Player.HasBuff(BuffID.ManaRegeneration))
        {
            Player.manaRegenDelay = 10;
        }
        Player.GetDamage(DamageClass.Magic) += baseMagicDamageBoost / 100f;
    }

    public override void PostUpdate()
    {

    }

    public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
    {
        //mult *= 1.5f;
    }

    public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
    {

    }

    public override void UpdateBadLifeRegen()
    {
        if (Player.HasBuff(BuffID.ManaRegeneration))
        {
            if (Player.lifeRegen > 0)
            {
                Player.lifeRegen = 0;
            }
            Player.lifeRegenTime = 0;
            Player.lifeRegen -= 10;
        }
    }
}