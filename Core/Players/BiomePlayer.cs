namespace TerrorMod.Core.Players;

public class BiomePlayer : ModPlayer
{
    public override void ResetEffects()
    {

    }

    public override void PostUpdate()
    {
        BiomeDebuffs();
    }

    void BiomeDebuffs()
    {
        if (Player.ZoneJungle)
        {
            Player.AddBuff(BuffID.Darkness, 2);
            Player.AddBuff(BuffID.Wet, 2);
        }
    }
}