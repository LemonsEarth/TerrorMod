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

    public class DeerclopsNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedDeerclops;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }

    public class DestroyerNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedMechBoss1;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }

    public class TwinsNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedMechBoss2;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }

    public class PrimeNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedMechBoss3;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }

    public class QueenSlimeNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedQueenSlime;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }

    public class PlanteraNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedPlantBoss;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }

    public class GolemNotDowned : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !NPC.downedGolemBoss;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }
}
