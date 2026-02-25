using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Buffs.Debuffs.Phobias;

public class AcrophobiaDebuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        LemonUtils.AddPhobiaDebuffs(player, 1.2f);
    }
}
