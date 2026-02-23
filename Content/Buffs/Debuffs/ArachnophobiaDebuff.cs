using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Buffs.Debuffs;

public class ArachnophobiaDebuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        LemonUtils.AddPhobiaDebuffs(player);
    }
}
