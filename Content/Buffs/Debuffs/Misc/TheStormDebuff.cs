namespace TerrorMod.Content.Buffs.Debuffs.Misc;

public class TheStormDebuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }
}
