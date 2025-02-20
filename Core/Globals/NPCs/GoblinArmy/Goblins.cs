using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorMod.Core.Globals.NPCs.GoblinArmy
{
    public class Goblins : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC entity)
        {
            if (entity.type == NPCID.GoblinThief)
            {
                entity.Opacity = 0.3f;
            }

            if (entity.type == NPCID.GoblinWarrior)
            {
                entity.knockBackResist = 0f;
            }
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.GoblinArcher
                || entity.type == NPCID.GoblinPeon
                || entity.type == NPCID.GoblinScout
                || entity.type == NPCID.GoblinShark
                || entity.type == NPCID.GoblinSorcerer
                || entity.type == NPCID.GoblinSummoner
                || entity.type == NPCID.GoblinWarrior
                || entity.type == NPCID.ChaosBall;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            int chanceDenominator = npc.type == NPCID.GoblinThief ? 1 : 4;
            if (Main.rand.NextBool(chanceDenominator))
            {
                Item heldItem = target.HeldItem;
                target.DropItem(npc.GetSource_OnHit(target), target.Center, ref heldItem);
            }
        }
    }
}
