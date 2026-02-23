using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Core.Players;

public class BuffPlayer : ModPlayer
{
    public override void ResetEffects()
    {

    }

    public override bool CanUseItem(Item item)
    {
        return !Player.HasBuff(BuffID.OgreSpit) && !Player.HasBuff(BuffType<FearDebuff>());
    }

    public override void PostUpdate()
    {
        if (Player.HasBuff(BuffID.ChaosState))
        {
            if (TerrorPlayer.Timer % 60 == 0)
            {
                Player.Teleport(Player.Center + Main.rand.NextVector2Circular(800, 800), 1);
            }
        }
    }
}