using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace TerrorMod.Common.Conditions
{
    public class KingSlimeNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedSlimeKing;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }

    public class BeeNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedQueenBee;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }

    public class SkeletronNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedBoss3;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }
}
