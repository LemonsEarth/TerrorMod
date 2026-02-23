using System.Linq;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Buffs.Debuffs;

public class Overdosed : ModBuff
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

public class OverdosedPlayer : ModPlayer
{
    void OverdosedCheckAndApply()
    {
        int buffLimit = 4;

        if (NPC.downedBoss1) buffLimit++;
        if (NPC.downedQueenBee) buffLimit += 2;
        if (NPC.downedBoss3) buffLimit++;
        if (NPC.downedDeerclops) buffLimit++;
        if (NPC.downedPirates) buffLimit += 2;
        if (NPC.downedQueenSlime) buffLimit += 2;
        if (NPC.downedHalloweenKing) buffLimit += 2;
        if (NPC.downedChristmasIceQueen) buffLimit += 2;
        if (NPC.downedAncientCultist) buffLimit += 2;

        if (Player.buffType.Where(buff => buff != 0 && Main.debuff[buff] == false).Count() > buffLimit)
        {
            Player.AddBuff(BuffType<Overdosed>(), 2);
        }
    }

    public override void PostUpdateBuffs()
    {
        OverdosedCheckAndApply();
    }

    public override void UpdateBadLifeRegen()
    {
        if (Player.HasBuff(BuffType<Overdosed>()))
        {
            Player.lifeRegen -= 7;
        }
    }
}
