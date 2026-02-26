using System.Linq;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Buffs.Debuffs.Temperature;

public class SuperHotDebuff : ModBuff
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
        player.moveSpeed *= 0.7f;
        player.tileSpeed -= 0.3f;
        player.wallSpeed -= 0.3f;
        player.pickSpeed += 0.66f;
    }
}

public class SuperHotDebuffPlayer : ModPlayer
{
    public override void UpdateBadLifeRegen()
    {
        if (Player.HasBuff(BuffType<SuperHotDebuff>()))
        {
            Player.DOTDebuff(1);
        }
    }
}
