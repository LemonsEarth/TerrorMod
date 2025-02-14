using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Core.Players;

namespace TerrorMod.Content.Buffs.Debuffs
{
    public class Overdosed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TerrorPlayer>().overdosed = true;
            LemonUtils.AddPhobiaDebuffs(player, 2f);
        }
    }
}
