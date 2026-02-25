namespace TerrorMod.Content.Buffs.Debuffs.Misc;

public class GunpowderedSnowDebuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        BuffID.Sets.LongerExpertDebuff[Type] = false;
        Main.buffNoTimeDisplay[Type] = true;
    }
}
