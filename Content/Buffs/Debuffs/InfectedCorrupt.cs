namespace TerrorMod.Content.Buffs.Debuffs;

public class InfectedCorrupt : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        BuffID.Sets.LongerExpertDebuff[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }
}
