using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TerrorMod.Core.Globals.NPCs.Corruption;
using TerrorMod.Core.Players;

namespace TerrorMod.Content.Buffs.Debuffs
{
    public class CurseDebuff : ModBuff
    {
        int curseLevel = 0;
        public override LocalizedText Description => base.Description.WithFormatArgs(curseLevel);

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            curseLevel = player.GetModPlayer<TerrorPlayer>().curseLevel;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = base.Description.WithFormatArgs(curseLevel).Value;
            rare = ItemRarityID.Orange;
        }
    }
}
