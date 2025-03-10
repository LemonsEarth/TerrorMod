using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Conditions;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Items.Consumables;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class Boss : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.boss;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.KingSlime:
                    ItemDropWithConditionRule rule1 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 4, 8, new KingSlimeNotDowned());
                    npcLoot.Add(rule1);
                    break;
                case NPCID.QueenBee:
                    ItemDropWithConditionRule rule2 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 4, 8, new BeeNotDowned());
                    npcLoot.Add(rule2);
                    break;
                case NPCID.SkeletronHead:
                    ItemDropWithConditionRule rule3 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 4, 8, new SkeletronNotDowned());
                    npcLoot.Add(rule3);
                    break;
                case NPCID.Deerclops:
                    ItemDropWithConditionRule rule4 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 4, 8, new DeerclopsNotDowned());
                    npcLoot.Add(rule4);
                    break;
                case NPCID.TheDestroyer:
                    ItemDropWithConditionRule rule5 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 4, 8, new DestroyerNotDowned());
                    npcLoot.Add(rule5);
                    break;
                case NPCID.SkeletronPrime:
                    ItemDropWithConditionRule rule6 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 4, 8, new PrimeNotDowned());
                    npcLoot.Add(rule6);
                    break;
                case NPCID.QueenSlimeBoss:
                    ItemDropWithConditionRule rule7 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 4, 8, new QueenSlimeNotDowned());
                    npcLoot.Add(rule7);
                    break;
                case NPCID.Plantera:
                    ItemDropWithConditionRule rule8 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 12, 18, new PlanteraNotDowned());
                    npcLoot.Add(rule8);
                    break;
                case NPCID.Golem:
                    ItemDropWithConditionRule rule9 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 4, 8, new GolemNotDowned());
                    npcLoot.Add(rule9);
                    break;
            }        
        }
    }
}
