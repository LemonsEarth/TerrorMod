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

namespace TerrorMod.Core.Globals.Items
{
    public class TerrorItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
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
