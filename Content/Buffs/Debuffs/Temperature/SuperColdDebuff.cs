using System.Linq;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Buffs.Debuffs.Temperature;

public class SuperColdDebuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        BuffID.Sets.LongerExpertDebuff[Type] = false;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.moveSpeed *= 0.6f;
        player.tileSpeed -= 0.6f;
        player.wallSpeed -= 0.6f;
        player.GetAttackSpeed(DamageClass.Generic) -= 0.4f;
    }
}

public class SuperColdDebuffPlayer : ModPlayer
{
    public override void UpdateBadLifeRegen()
    {
        if (Player.HasBuff(BuffType<SuperColdDebuff>()))
        {
            Player.DOTDebuff(1);
        }
    }
}
