using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Core.Players;
namespace TerrorMod.Content.Buffs.Buffs
{
    public class BeforeTheStormBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.persistentBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }
    }
}
