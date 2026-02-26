using System.Linq;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Buffs.Debuffs.Temperature;

public class ColdDebuff : ModBuff
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
        player.moveSpeed *= 0.8f;
        player.tileSpeed -= 0.4f;
        player.wallSpeed -= 0.4f;
        player.GetAttackSpeed(DamageClass.Generic) -= 0.15f;
    }
}
