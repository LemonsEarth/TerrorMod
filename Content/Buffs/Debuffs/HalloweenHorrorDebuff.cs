namespace TerrorMod.Content.Buffs.Debuffs;

public class HalloweenHorrorDebuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        BuffID.Sets.LongerExpertDebuff[Type] = false;
        Main.buffNoTimeDisplay[Type] = true;
    }
}
