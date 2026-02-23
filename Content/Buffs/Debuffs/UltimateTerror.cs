using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Buffs.Debuffs;

public class UltimateTerror : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        LemonUtils.AddPhobiaDebuffs(player, 2f);
    }
}

public class UltimateTerrorPlayer : ModPlayer
{
    public override void UpdateBadLifeRegen()
    {
        if (Player.HasBuff(BuffType<UltimateTerror>()))
        {
            if (Player.lifeRegen > 0)
            {
                Player.lifeRegen = 0;
            }
            Player.lifeRegenTime = 0;
            Player.lifeRegen -= 150;
        }
    }
}
