using System.Linq;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Buffs.Debuffs;

public class ManaMalady : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        BuffID.Sets.LongerExpertDebuff[Type] = false;
    }
}
