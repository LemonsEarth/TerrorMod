using Terraria;
using Terraria.ModLoader;
using TerrorMod.Core.Players;
namespace TerrorMod.Content.Buffs.Buffs
{
    public class TungstenPenetration : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
