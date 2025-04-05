using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs;
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Corruption;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.Items
{
    public class TerrorItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void OnConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            if (!SkullSystem.gluttonySkullActive) return;
            if (ammo.stack > 1) ammo.stack--;
        }

        public override void OnConsumeMana(Item item, Player player, int manaConsumed)
        {
            if (!SkullSystem.gluttonySkullActive) return;
            player.CheckMana(manaConsumed, true);
        }

        public override void SetStaticDefaults()
        {
            for (int i = 0; i < ContentSamples.ItemsByType.Count; i++)
            {
                if (ItemID.Sets.ShimmerTransformToItem[i] > 0)
                {
                    continue;
                }
                ItemID.Sets.ShimmerTransformToItem[i] = ItemID.Seagull;
            }
        }
    }
}
