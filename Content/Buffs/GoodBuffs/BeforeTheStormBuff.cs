namespace TerrorMod.Content.Buffs.GoodBuffs;

public class BeforeTheStormBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.persistentBuff[Type] = true;
        Main.buffNoSave[Type] = false;
    }
}
