using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Content.Buffs.Buffs;

public class AntidoteBuff : ModBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.buffImmune[BuffType<InfectedCorrupt>()] = true;
        player.buffImmune[BuffType<InfectedCrimson>()] = true;
    }
}
