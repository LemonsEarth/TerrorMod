using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Buffs;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;

namespace TerrorMod.Content.Items.Special
{
    public class LootToken : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 999;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Orange;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            IItemDropRule bombRule = ItemDropRule.Common(ItemID.Bomb, 1, 5, 5);
            IItemDropRule grenadeRule = ItemDropRule.Common(ItemID.Grenade, 1, 15, 15);
            IItemDropRule campRule = ItemDropRule.Common(ItemID.Campfire, 1, 1, 1);
            IItemDropRule swordRule = ItemDropRule.Common(ItemID.CopperBroadsword, 1, 1, 1);
            IItemDropRule regenRule = ItemDropRule.Common(ItemID.BandofRegeneration, 1, 1, 1);
            itemLoot.Add(new OneFromRulesRule(1, bombRule, grenadeRule, campRule, swordRule, regenRule));
        }
    }
}
