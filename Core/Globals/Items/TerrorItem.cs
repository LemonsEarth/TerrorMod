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

        public override void UpdateInventory(Item item, Player player)
        {
            if (!SkullSystem.gluttonySkullActive) return;
            if (item.ammo != AmmoID.None && item.stack > 999)
            {
                player.moveSpeed -= (item.stack / 1000) * 0.05f;
            }
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            if (!SkullSystem.gluttonySkullActive) return;
            mult = 2;
        }

        public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
        {
            if (SkullSystem.greedSkullActive)
            {
                canApplyDiscount = false;
                reforgePrice *= 2;
                return false;
            }
            return true;
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
