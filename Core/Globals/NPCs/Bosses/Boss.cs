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
                    ItemDropWithConditionRule rule1 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 2, 4, new KingSlimeNotDowned());
                    npcLoot.Add(rule1);
                    break;
                case NPCID.QueenBee:
                    ItemDropWithConditionRule rule2 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 2, 4, new BeeNotDowned());
                    npcLoot.Add(rule2);
                    break;
                case NPCID.SkeletronHead:
                    ItemDropWithConditionRule rule3 = new ItemDropWithConditionRule(ModContent.ItemType<LifeEssence>(), 1, 2, 4, new SkeletronNotDowned());
                    npcLoot.Add(rule3);
                    break;
            }        
        }
    }
}
